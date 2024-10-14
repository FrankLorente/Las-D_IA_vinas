using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer;
    public int VisionConeResolution = 120;
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;

    public bool playerDetected = false;
    public bool visto = false;
    void Start()
    {
        transform.AddComponent<MeshRenderer>().material = VisionConeMaterial;
        MeshFilter_ = transform.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
    }

    
    void Update()
    {
        DrawVisionCone();
    }

    void DrawVisionCone()
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] Vertices = new Vector3[VisionConeResolution + 1];
        Vertices[0] = Vector3.zero;
        float Currentangle = -VisionAngle / 2;
        float angleIcrement = VisionAngle / (VisionConeResolution - 1);
        float Sine;
        float Cosine;
        //bool playerDetected = false; // Variable para saber si se ha detectado al jugador

        for (int i = 0; i < VisionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
            
            
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer))
            {
                Vertices[i + 1] = VertForward * hit.distance;
                //playerDetected = hit.collider.CompareTag("Player");
                
                // Verificar si el objeto golpeado es el jugador


                if (hit.collider.CompareTag("Player") == true)
                {
                    //visto = true;
                    //Debug.Log("JUGADOR DETECTADO");
                    //visto = true;
                    playerDetected = true;
                    //Debug.Log(playerDetected);
                    
                    //Debug.Log(playerDetected);

                }

                /*
                else
                {
                    //visto = false; // Se ha detectado al jugador
                    //Debug.Log("JUGADOR NO DETECTADO");
                    //Debug.Log(playerDetected);
                    //visto = false;
                }
                */


            }
            else
            {
                Vertices[i + 1] = VertForward * VisionRange;
                if(playerDetected == true)
                {
                    playerDetected = false;
                }
                
                //Debug.Log(playerDetected);
            }

            

            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        //VisionConeMesh.Clear();
        VisionConeMesh.vertices = Vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;

    }
}