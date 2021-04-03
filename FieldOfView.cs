using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0f, 360f)] public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public List<Transform> visibleTarget = new List<Transform>();

    public float delayTime;

    private void Start()
    {
        StartCoroutine("FindTargetWithDelay", delayTime);
    }

    IEnumerator FindTargetWithDelay(float delayTime)
    {
        // Start the method until next scene. So it is always to be true.
        while (true)
        {
            yield return new WaitForSeconds(delayTime);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTarget.Clear();

        Collider2D[] targetInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for(var i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector2 dirToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right,dirToTarget) < viewAngle / 2)
            {
                float disTotarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, disTotarget, obstacleMask))
                {
                    visibleTarget.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }

        return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }
}
