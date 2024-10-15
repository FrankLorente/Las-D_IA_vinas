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

        else if(isDetected && (!Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor)))// || (visionCone != null && !visionCone.playerDetected)))// && !Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor))
        {
            isDetected = false;
            //visionCone.visto = false;
            Debug.Log("Something is NOT here");
        }

        else if(!isDetected)
        {
            if(Physics.CheckSphere(transform.position, sphereRadius, layerPatrol))
            {
                if(unit.target == unit.listaPuntos[0])
                {
                    unit.target = unit.listaPuntos[1];
                }

                else if (unit.target == unit.listaPuntos[1])
                {
                    unit.target = unit.listaPuntos[0];
                }

                else if (unit.target == unit.player)
                {
                    unit.target = unit.listaPuntos[0];
                }
                //unit.Patrullar(unit.listaPuntos);
            }
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