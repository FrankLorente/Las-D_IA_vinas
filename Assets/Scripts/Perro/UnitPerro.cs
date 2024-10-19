using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPerro : MonoBehaviour
{
    // esta clase hereda de Unit porque no requiere ninguna de su funcionalidad

    public Transform _origenLadrido;
    public float _radioSonido = 10.0f;
    public LayerMask _mascaraEnemigos;

    private ConoDeVision _conoVision;

    void Start()
    {
        _conoVision = GetComponentInChildren<ConoDeVision>();
    }

    void Update()
    {
        if (_conoVision.detected)
        {
            // cambia estado a alarma
            // Debug.Log("WOOF WOOF");
            Ladrar();
        }
        else
        {
            // cambia estado a idle
            // Debug.Log("ZzZzz");
        }
    }

    private void Ladrar()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, _radioSonido, _mascaraEnemigos);

        foreach(Collider col in colliders)
        {
            GameObject cultistaActual = col.gameObject;
            cultistaActual.GetComponent<DetectionCultist>().HearNoise(_origenLadrido);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _radioSonido);
    }
}
