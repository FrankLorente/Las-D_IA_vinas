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


        if (other.gameObject == player && TieneLlave)
        {
            m_IsPlayerAtExit = true;
            Debug.Log("El jugador ha llegado a la salida y tiene la llave.");
        }

        if (Llave == player)
        {
            TieneLlave = true;
            Llave.SetActive(false);
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
        /*
        m_Timer += Time.deltaTime;

        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Debug.Log("Fin del nivel.");
            Application.Quit();
        }
        */

        // Incrementa el temporizador con el tiempo
        m_Timer += Time.deltaTime;

        // Calcula el valor alpha basado en el tiempo transcurrido
        float alphaValue = m_Timer / fadeDuration;

        // Asegúrate de que el alpha no sea mayor que 1
        exitBackgroundImageCanvasGroup.alpha = Mathf.Clamp01(alphaValue);

        // Una vez que el tiempo transcurrido supere la duración del fade y la duración de la imagen, finaliza el juego
        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Debug.Log("Final.");
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit(); // Termina el juego
        }
    }
}
