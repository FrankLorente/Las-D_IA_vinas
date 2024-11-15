using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/* //DAVID CODIGO
public class DetectionCultist : Detection
{
    public Unit unit;
    public float _deathDistance = 1f;   // si el cultista consigue acercarse lo suficiente al jugador este muere
    public float _safeDistance = 10f;   // si el jugador consigue alejarse lo suficiente el cultista deja de seguirle

    // Fade Out
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    private float m_Timer;


    private Transform _noiseOrigin;
    private bool _loseGame = false;

    public float distanciaMinimaWaypoint = 1.5f;

    // FSM
    [HideInInspector] public enum State
    {
        idle,
        patrol,
        check,
        chase
    }
    private State _currentState;
    private State _previousState;

    private void Start()
    {
        visionCone = GetComponentInChildren<ConoDeVision>();

        _currentState = State.idle;
        _previousState = _currentState;

        GameObject canvasObject = GameObject.Find("Final");
    }

    void Update()
    {
        if (visionCone.detected)
        {
            ChangeState(State.chase);
            //unit.previousTarget = unit.target;
            unit.target = unit.player;
        }


        switch (_currentState)
        {
            case State.chase:
                Chase();
                break;
            case State.patrol:
                Patrol();
                break;
            case State.check:
                Check();
                break;
            default:
                Idle();
                break;
        }

        if (_loseGame)
        {
            LoseGame();
        }
    }

    private void ChangeState(State newState)
    {
        if(_currentState != newState)    
            _previousState = _currentState;
        _currentState  = newState;
    }

    private void Idle()
    {
        // este estado es solo por siacaso, realmente no vale de nada
        if (unit.listaPuntos.Count > 0)
            ChangeState(State.patrol);
    }

    private void Patrol()
    {
        if (Physics.CheckSphere(transform.position, sphereRadiusPatrol, layerPatrol))
        {
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(false);
            if (unit.siguienteWaypoint < unit.listaPuntos.Count - 1)
                unit.siguienteWaypoint++;
            else
                unit.siguienteWaypoint = 0;
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(true);

            unit.target = unit.listaPuntos[unit.siguienteWaypoint].transform;
            unit.previousTarget = unit.target;
        }
    }
    private void Check()
    {
        if (Vector3.Distance(this.transform.position, _noiseOrigin.position) < 2f)
        {
            // esta lo bastante cerca como para haber visto al jugador
            // en un mundo ideal el cultista se queda quieto por unos segundos, soy muy vago para implementarlo ahora mismo

            // si hubiese visto al jugador ya estaria en estado de chase, solo necesito volver al estado anterior
            _currentState = _previousState;
            unit.target = unit.previousTarget;
        }
    }

    private void Chase()
    {
        unit.target = unit.player;

        if(Vector3.Distance(this.transform.position, unit.player.position) < _deathDistance)
        {
            // el jugador se ha acercado lo suficiente como para ser atrapado
            //LoseGame();
            _loseGame = true;
        } else if (Vector3.Distance(this.transform.position, unit.player.position) > _safeDistance)
        {
            // el jugador se ha alejado lo suficiente como para escapar ... por el momento muahahahahaa
            _currentState = _previousState;
            unit.target = unit.previousTarget;
        }
    }

    public void HearNoise(Transform noiseOrigin)
    {
        ChangeState(State.check);

        foreach (GameObject punto in unit.listaPuntos)
            if (punto.transform == unit.target)
                unit.previousTarget = punto.transform;

        unit.target  = noiseOrigin;
        _noiseOrigin = noiseOrigin;
    }

    private void LoseGame() // esto solo lo necesitan los cultistas
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
*/

//PERE PRUEBA

public class DetectionCultist : Detection
{
    public Unit unit;
    public float _deathDistance = 1f;   // si el cultista consigue acercarse lo suficiente al jugador este muere
    public float _safeDistance = 10f;   // si el jugador consigue alejarse lo suficiente el cultista deja de seguirle

    // Fade Out
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    private float m_Timer;


    private Transform _noiseOrigin;
    private bool _loseGame = false;

    public float distanciaMinimaWaypoint = 1.5f;

    // FSM
    [HideInInspector]
    public enum State
    {
        idle,
        patrol,
        check,
        chase
    }
    private State _currentState;
    private State _previousState;

    private void Start()
    {
        visionCone = GetComponentInChildren<ConoDeVision>();

        _currentState = State.idle;
        _previousState = _currentState;

        GameObject canvasObject = GameObject.Find("Final");
    }

    void Update()
    {
        if (visionCone.detected)
        {
            ChangeState(State.chase);
            //unit.target = unit.player;
        }


        switch (_currentState)
        {
            case State.chase:
                Chase();
                break;
            case State.patrol:
                Patrol();
                break;
            case State.check:
                Check();
                break;
            default:
                Idle();
                break;
        }

        if (_loseGame)
        {
            LoseGame();
        }
    }

    private void ChangeState(State newState)
    {

        if (_currentState != newState)
        {
            _currentState = newState;
        }

    }

    private void Idle()
    {
        // este estado es solo por siacaso, realmente no vale de nada
        if (unit.listaPuntos.Count > 0)
            ChangeState(State.patrol);
    }

    private void Patrol()
    {
        if (Physics.CheckSphere(transform.position, sphereRadiusPatrol, layerPatrol))
        {
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(false);
            if (unit.siguienteWaypoint < unit.listaPuntos.Count - 1)
                unit.siguienteWaypoint++;
            else
                unit.siguienteWaypoint = 0;
            unit.listaPuntos[unit.siguienteWaypoint].SetActive(true);

            unit.target = unit.listaPuntos[unit.siguienteWaypoint].transform;
            unit.previousTarget = unit.target;
        }
    }
    private void Check()
    {
        if (Vector3.Distance(this.transform.position, _noiseOrigin.position) < 2f)
        {
            // esta lo bastante cerca como para haber visto al jugador
            // en un mundo ideal el cultista se queda quieto por unos segundos, soy muy vago para implementarlo ahora mismo

            // si hubiese visto al jugador ya estaria en estado de chase, solo necesito volver al estado anterior
            _currentState = _previousState;
            unit.target = unit.previousTarget;
        }
    }

    private void Chase()
    {
        unit.target = unit.player;

        if (Vector3.Distance(this.transform.position, unit.player.position) < _deathDistance)
        {
            _loseGame = true;
        }
        else if (Vector3.Distance(this.transform.position, unit.player.position) > _safeDistance)
        {
            _currentState = _previousState;
            unit.target = unit.previousTarget;
        }
    }

    public void HearNoise(Transform noiseOrigin)
    {
        if (_currentState != State.chase) //PERE HA PUESTO ESTO
        {
            ChangeState(State.check);
        }

        //ChangeState(State.check); //DAVID LO TENIA ASI
        /*
        foreach (GameObject punto in unit.listaPuntos)
            if (punto.transform == unit.target)
                unit.previousTarget = punto.transform;
        */
        unit.target = noiseOrigin;
        _noiseOrigin = noiseOrigin;
    }


    private void LoseGame() // esto solo lo necesitan los cultistas
    {
        m_Timer += Time.deltaTime;
        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            Debug.Log("Fin del nivel.");
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();
            SceneManager.LoadScene("Dungeon_Demo");
        }
    }
}
