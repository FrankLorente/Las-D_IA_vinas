using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public float sphereRadius = 5;
    public LayerMask layerWeAreLookingFor;
    public bool isDetected = false;
    /*
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    */
    void Update()
    {
        // Play a noise if an object is within the sphere's radius.
        //playerCollision = Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor);
        
        if (!isDetected && Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor))//Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor, QueryTriggerInteraction.Collide))
        {
            isDetected = true;
            Debug.Log("Something is here");
        }

        else if(isDetected && !Physics.CheckSphere(transform.position, sphereRadius, layerWeAreLookingFor))
        {
            isDetected = false;
            Debug.Log("Something is NOT here");
        }

        //else
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }

}