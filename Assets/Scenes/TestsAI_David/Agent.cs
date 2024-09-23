using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    // Atributos del vehículo
    public float mass;              // Masa del vehículo
    public Vector3 velocity;        // Velocidad actual
    public float rotationSpeed;
    public float maxForce;          // Fuerza máxima aplicable
    public float maxSpeed;          // Velocidad máxima permitida
    public Vector3[] orientation;   // Vector de orientación (N basis vectors)
    public Transform target;
    private float ypos;

    public Agent(float mass, float rotationSpeed,  float maxForce, float maxSpeed, Transform target)
    {
        this.mass = mass;
        this.velocity = Vector3.zero; // Inicializa con velocidad 0
        this.rotationSpeed = rotationSpeed;
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

    private void Start()
    {
        this.ypos = this.transform.position.y;
    }

    private void Update()
    {
        Vector3 steeringDirection = target.position - this.transform.position;

        //Forward Euler Integration
        //Vector3 steeringForce = Truncate(steeringDirection, maxForce);
        Vector3 steeringForce = Truncate(Seek(), maxForce);
        Vector3 acceleration = steeringForce / mass;
        velocity = Truncate(velocity + acceleration*Time.deltaTime, maxSpeed);

        this.transform.position = this.transform.position + velocity*Time.deltaTime;
        this.transform.position = new Vector3(this.transform.position.x,
                                              ypos,
                                              this.transform.position.z);

        // Optional: Rotate towards the target
        //if (velocity != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(velocity);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        //}

        steeringDirection.y = 0.0f;
        // If the direction is not zero, calculate the desired rotation
        if (steeringDirection.magnitude > 0.01f)
        {
            // Calculate the target rotation towards the target (only y-axis)
            Quaternion targetRotation = Quaternion.LookRotation(steeringDirection);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
