using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public float sphereRadiusPatrol = 1.0f;
    public LayerMask layerWeAreLookingFor;
    public LayerMask layerPatrol;
    [HideInInspector] public ConoDeVision visionCone;
}