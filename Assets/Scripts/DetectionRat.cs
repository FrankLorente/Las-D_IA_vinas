using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetectionRat : MonoBehaviour
{
    public float sphereRadius = 5.0f;
    public LayerMask layerWeAreLookingFor;
    public LayerMask layerPatrol;
    public bool isDetected = false;

    private VisionCone visionCone; // Agregamos una variable para almacenar la referencia al script del hijo
    public UnitRat unit;

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        visionCone = GetComponentInChildren<VisionCone>();
        unit = GetComponent<UnitRat>();
    }
    
    void Update()
    {
        // Play a noise if an object is within the sphere's radius.

        if (!isDetected && (Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor)))// || (visionCone != null && visionCone.playerDetected)))//Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor, QueryTriggerInteraction.Collide))
        {
            isDetected = true;
            //visionCone.visto = true;
            Debug.Log("Something is here");
        }

        else if (isDetected && (!Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor)))// || (visionCone != null && !visionCone.playerDetected)))// && !Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor))
        {
            isDetected = false;
            //visionCone.visto = false;
            Debug.Log("Something is NOT here");
        }

        else if(Physics.CheckSphere(transform.position, sphereRadius, layerPatrol))
        {
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(false);
            if (unit.siguienteWaypoint < unit.listaPuntos.Count - 1)
                unit.siguienteWaypoint++;
            else
                unit.siguienteWaypoint = 0;
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(true);
        }

        //else if (visionCone != null)
        //{
        /*
            if (!isDetected && visionCone.playerDetected)
            {
                isDetected = true;
            }

            else if(isDetected && !visionCone.playerDetected)
            {
                isDetected = false;
            }
            */
        //}
        //else
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }

}