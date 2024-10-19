using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public float sphereRadius = 5.0f;
    public float sphereRadiusPatrol = 1.0f;
    public LayerMask layerWeAreLookingFor;
    public LayerMask layerPatrol;
    public bool isDetected = false;

    private ConoDeVision visionCone; // Agregamos una variable para almacenar la referencia al script del hijo

    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        visionCone = GetComponentInChildren<ConoDeVision>();
    }
    
    public virtual void MiUpdate()
    {
        // Play a noise if an object is within the sphere's radius.

        
        if (!isDetected && (Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor)))// || (visionCone != null && visionCone.playerDetected)))//Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor, QueryTriggerInteraction.Collide))
        {
            isDetected = true;
            //visionCone.visto = true;
            Debug.Log("Something is here");
        }

        else if(isDetected && (!Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor)))// || (visionCone != null && !visionCone.playerDetected)))// && !Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor))
        {
            isDetected = false;
            //visionCone.visto = false;
            Debug.Log("Something is NOT here");
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