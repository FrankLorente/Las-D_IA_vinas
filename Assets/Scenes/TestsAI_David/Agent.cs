using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    // Atributos del vehículo
    public float mass;              // Masa del vehículo
    public Vector3 velocity;        // Velocidad actual
    public float maxForce;          // Fuerza máxima aplicable
    public float maxSpeed;          // Velocidad máxima permitida
    public Vector3[] orientation;   // Vector de orientación (N basis vectors)
    public Transform target;

    public Agent(float mass,  float maxForce, float maxSpeed, Transform target)
    {
        this.mass = mass;
        this.velocity = Vector3.zero; // Inicializa con velocidad 0
        this.maxForce = maxForce;
        this.maxSpeed = maxSpeed;
        this.orientation = new Vector3[3]; // Puedes definir tres vectores base (e.g. up, right, forward)
        SetDefaultOrientation();

        this.target = target;
    }

    // Método para establecer una orientación básica inicial
    private void SetDefaultOrientation()
    {
        orientation[0] = Vector3.forward; // Dirección hacia adelante
        orientation[1] = Vector3.up;      // Dirección hacia arriba
        orientation[2] = Vector3.right;   // Dirección hacia la derecha
    }

    public Vector3 Truncate(Vector3 vector, float maxMagnitude)
    {
        // Check if the vector's magnitude exceeds the maximum allowed magnitude
        if (vector.magnitude > maxMagnitude)
        {
            // Normalize the vector and scale it to the maximum allowed magnitude
            return vector.normalized * maxMagnitude;
        }

        // If the magnitude is already within the limit, return the vector as is
        return vector;
    }

    private Vector3 Seek()
    {
        Vector3 desiredVelocity = (target.position - this.transform.position).normalized * maxSpeed;
        return desiredVelocity - this.velocity;
    }

    private void Update()
    {
        Vector3 steeringDirection = new Vector3(0f, 0f, 0f);

        //Forward Euler Integration
        //Vector3 steeringForce = Truncate(steeringDirection, maxForce);
        Vector3 steeringForce = Truncate(Seek(), maxForce);
        Vector3 acceleration = steeringForce / mass;
        velocity = Truncate(velocity + acceleration*Time.deltaTime, maxSpeed);

        this.transform.position = this.transform.position + velocity*Time.deltaTime;

        // Optional: Rotate towards the target
        if (velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
