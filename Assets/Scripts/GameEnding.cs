using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public GameObject Llave;
    public CanvasGroup exitBackgroundImageCanvasGroup;

    bool m_IsPlayerAtExit;
    float m_Timer;

    public bool TieneLlave = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión detectada con: " + other.gameObject.name);


        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
            Debug.Log("El jugador ha llegado a la salida y tiene la llave.");
        }
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel();
        }
    }

    void EndLevel()
    {
        Debug.Log("Fin del nivel2.");

        m_Timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Debug.Log("Fin del nivel.");
            Application.Quit();
        }
        
    }
}
