using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform player;
    public List<GameObject> listaPuntos;
    [HideInInspector] public Transform target;
    [HideInInspector] public Transform previousTarget;
    public float speed = 20;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 10;
    private Detection playerDetection;
    Path path;

    [HideInInspector] public int siguienteWaypoint = 0;
    public bool detectado;


    Animator m_Animator;


    void Start()
    {
        MyStart();
    }
    private void Update()
    {
        MyUpdate(); 
    }
    protected virtual void MyStart()
    {
        StartCoroutine(UpdatePath());
        playerDetection = GetComponent<Detection>();

        if (listaPuntos.Count > 0)
        {
            listaPuntos[0].SetActive(true);
            for (int i = 1; i < listaPuntos.Count; i++)
            {
                listaPuntos[i].gameObject.SetActive(false);
            }

            target = listaPuntos[0].transform;
            previousTarget = target;
        }

        m_Animator = GetComponent<Animator>();

    }

    protected virtual void MyUpdate()
    {
        if (speed > 0)
        {
            m_Animator.SetBool("IsWalking", true);
        }
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if(playerDetection.visionCone.detected) //Si se detecta al personaje principal, que el enemigo haga un camino hasta él
        {
            if (pathSuccessful) //Una vez el camino sea valido(comprobado usando A*)
            {
                path = new Path(waypoints, transform.position, turnDst, stoppingDst);

                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        else
        {
            if (pathSuccessful) //Una vez el camino sea valido(comprobado usando A*)
            {
                path = new Path(waypoints, transform.position, turnDst, stoppingDst);

                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }
    }

    IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }

        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
            }

            //Debug.Log("Enemigo siguiendo camino");

            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}