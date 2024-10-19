using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = .5f;  // Nueva variable para la velocidad de movimiento
    public float turnSpeed = 10f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    AudioSource m_AudioSource;

    public float pickUpRange = 2.0f;
    public GameObject itemKey;

    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public bool m_IsPlayerAtExit = false;
    float m_Timer;
    public bool TieneLlave = false;
    public GameObject triggerGameEnding;

    //public GameEnding gameEnding;
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        //gameEnding = FindObjectOfType<gameEnding>();
        triggerGameEnding.GetComponent<MeshRenderer>().enabled = false;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            // Calcular la direcci�n a la que el personaje debe mirar
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        if (m_IsPlayerAtExit)
        {
            EndLevel();


            //EndLevel();
        }

    }

    void OnAnimatorMove()
    {
        
        // Aqu� ya no dependemos de la animaci�n para el movimiento, usamos la velocidad que definimos.
        Vector3 moveDirection = m_Movement * moveSpeed * Time.deltaTime;

        // Mover el personaje
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveDirection);
        

        //m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);

        // Aplicar la rotaci�n
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))  // Asegúrate de que el objeto tenga esta etiqueta
        {
            TieneLlave = true;
            Debug.Log("Coge llave");
            itemKey.SetActive(false);
        }
        if (other.CompareTag("ExitTrigger") && TieneLlave)
        {
            //Debug.Log("Fin del nivel.");
            m_IsPlayerAtExit = true;
            
        }
    }

    void EndLevel()
    {
        
        m_Timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Debug.Log("Fin del nivel.");
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
