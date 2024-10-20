using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRat : Detection
{
    public Unit unit;

    public Transform _origenChillido;
    public float _radioSonido = 20f;
    public LayerMask _mascaraEnemigos;

    public AudioSource _ratAudio;
    public bool _hasSkreed = false;

    private void Start()
    {
        visionCone = GetComponentInChildren<ConoDeVision>();
    }

    void Update()
    {
        if (visionCone.detected)    // PÁNICO
        {
            if (!_hasSkreed)
            {
                _hasSkreed = true;
                _ratAudio.Play();
            }
            // ha detectado al jugador --> entra en pánico y se pone a chillar como la ratita que es
            Chillar();
        }
        else if(Physics.CheckSphere(transform.position, sphereRadiusPatrol, layerPatrol))   // PATRULLA
        {
            _hasSkreed = false;
            _ratAudio.Stop();

            unit.target = unit.previousTarget;

            unit.listaPuntos[unit.siguienteWaypoint].SetActive(false);
            if (unit.siguienteWaypoint < unit.listaPuntos.Count - 1)
                unit.siguienteWaypoint++;
            else
                unit.siguienteWaypoint = 0;
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(true);

            unit.target = unit.listaPuntos[unit.siguienteWaypoint].transform;
            unit.previousTarget = unit.target;
        }
    }
    
    private void Chillar()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _radioSonido, _mascaraEnemigos);

        foreach (Collider col in colliders)
        {
            GameObject cultistaActual = col.gameObject;
            cultistaActual.GetComponent<DetectionCultist>().HearNoise(_origenChillido);
        }
    }
}