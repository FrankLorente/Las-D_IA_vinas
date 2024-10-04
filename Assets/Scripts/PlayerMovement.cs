using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocityMov = 7.0f;
    public float velocityRot = 250.0f;

    private Animator anim;
    public float x, y;

    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        transform.Rotate(0, x * Time.deltaTime * velocityRot, 0);
        transform.Translate(0, 0, y * Time.deltaTime * velocityMov);

        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);
    }
}
