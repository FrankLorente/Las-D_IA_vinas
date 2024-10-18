using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConoDeVision : MonoBehaviour
{
	
	public float viewRadius;
	[Range(0, 360)]

	public float viewAngle;

	[SerializeField]
	public LayerMask targetMask;

	[SerializeField]
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();




    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    float m_Timer;
	bool detected = false;



    void Start()
	{
		StartCoroutine("FindTargetsWithDelay", .2f);

		GameObject canvasObject = GameObject.Find("Final");

		exitBackgroundImageCanvasGroup = canvasObject.GetComponent<CanvasGroup>();
	}

    private void Update()
    {
        if (detected)
		{
            LoseGame();

        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets()
	{

		visibleTargets.Clear();
		
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		
		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{

			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;

			if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2)
			{

				float dstToTarget = Vector3.Distance(transform.position, target.position);



				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					if (visibleTargets.Count == 0)
					{
						visibleTargets.Add(target);
						print("Detectado");
						//GetComponentInParent<Detection>().isDetected = true;
						Debug.Log("Jugador detected");

						detected = true;						
					}					
				}
				else
				{
					visibleTargets.Clear();
					print("No detectado");

				}
			}
		}
	}


	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}

	private void OnDrawGizmos()
	{
		float totalFOV = viewAngle;
		float rayRange = viewRadius;
		float halfFOV = totalFOV / 2.0f;
		Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV+90f, Vector3.up);
		Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV+90f, Vector3.up);
		Vector3 leftRayDirection = leftRayRotation * transform.forward;
		Vector3 rightRayDirection = rightRayRotation * transform.forward;
		Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
		Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
		Gizmos.DrawWireSphere(transform.position, rayRange);
		
	}


    void LoseGame()
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