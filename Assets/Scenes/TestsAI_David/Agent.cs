using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Agent : MonoBehaviour
{

    // Atributos del vehículo
    public float mass;                  // Masa del vehículo
    public Vector3 velocity;            // Velocidad actual
    public float rotationSpeed;
    public float maxForce;              // Fuerza máxima aplicable
    public float maxSpeed;              // Velocidad máxima permitida
    public float rayLength = 5f;        // Length of the ray for obstacle detection
    public float avoidForce = 20f;      // Force to steer away from obstacles
    public float detectionAngle = 45f;  // Angle of side rays
    public Vector3[] orientation;       // Vector de orientación (N basis vectors)
    public Transform target;
    public LayerMask layerMask;
    private float ypos;

    //private float count = 0;

    public Agent(float mass, float rotationSpeed,  float maxForce, float maxSpeed,
                 float rayLength, float avoidForce, float detectionAngle, Transform target, LayerMask layerMask)
    {
        this.mass = mass;
        this.velocity = Vector3.zero; // Inicializa con velocidad 0
        this.rotationSpeed = rotationSpeed;
        this.maxForce = maxForce;
        this.maxSpeed = maxSpeed;
        this.rayLength = rayLength;
        this.avoidForce = avoidForce;
        this.detectionAngle = detectionAngle;
        this.layerMask = layerMask;

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

    private Vector3 AvoidObstacles(Vector3 steeringForce)
    {
        Ray rayRight = new Ray(transform.position, Quaternion.AngleAxis(detectionAngle, transform.up)  * transform.forward);
        Ray rayLeft  = new Ray(transform.position, Quaternion.AngleAxis(-detectionAngle, transform.up) * transform.forward);

        Debug.DrawRay(transform.position, Quaternion.AngleAxis(detectionAngle, transform.up) * transform.forward * rayLength, Color.green);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-detectionAngle, transform.up) * transform.forward * rayLength, Color.blue);

        RaycastHit hit;

        if(Physics.Raycast(rayLeft, out hit, rayLength, layerMask))
        {
            steeringForce = hit.normal * avoidForce;
        } else if(Physics.Raycast(rayRight, out hit, rayLength, layerMask))
        {
            steeringForce = hit.normal * avoidForce;
        }

        return steeringForce;
    }

    private void Seek()
    {
        Vector3 steeringDirection = target.position - this.transform.position;

        //Forward Euler Integration
        Vector3 desiredVelocity = (target.position - this.transform.position).normalized * maxSpeed;
        Vector3 steeringForce = desiredVelocity - this.velocity;
        steeringForce = AvoidObstacles(steeringForce);
        steeringForce = Truncate(steeringForce, maxForce);
        Vector3 acceleration = steeringForce / mass;
        velocity = Truncate(velocity + acceleration * Time.deltaTime, maxSpeed);

        this.transform.position = this.transform.position + velocity * Time.deltaTime;
        this.transform.position = new Vector3(this.transform.position.x,
                                              ypos,
                                              this.transform.position.z);

        PointToTarget(steeringDirection);
    }
    private void PointToTarget(Vector3 steeringDirection)
    {
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

    private void Start()
    {
        this.ypos = this.transform.position.y;
    }

    private void Update()
    {
        Seek();
    }
}
