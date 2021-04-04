using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CameraMovement : MonoBehaviour
{
    #region Parameters
    private float angleTheta = 50f;
    private float lerpBackTime = 3f;
    private float lerpStareTime = .5f;
    private float lerpReturnTime = 3f;

    private bool nowMove = true;
    private bool nowStare = true;
    private bool wasStare = true;
    private bool backStare = true;

    private float nowAngleTheta;
    private float startTime;
    private float startStareTime;
    private float startReturnTime;
    #endregion

    private FieldOfView fow;

    // Start is called before the first frame update
    void Start()
    {
        nowAngleTheta = angleTheta;
        fow = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (nowMove && fow.visibleTarget.Count == 0)
        {
            angleTheta *= -1;
            nowMove = false;
            startTime = Time.fixedTime;
        }
        else if (!nowMove && fow.visibleTarget.Count == 0)
        {
            if (nowStare)
            {
                nowAngleTheta = fixedFloatLerp(nowAngleTheta, angleTheta, startTime, lerpBackTime);
                LookAt2D(transform.position, transform.position + new Vector3(Mathf.Cos((nowAngleTheta - 90f) * Mathf.Deg2Rad), Mathf.Sin((nowAngleTheta - 90f) * Mathf.Deg2Rad), 0f), startTime, lerpBackTime);
            }
            else
            {
                if (wasStare)
                {
                    wasStare = false;
                    startReturnTime = Time.fixedTime;
                }
                else
                {
                    if (!backStare)
                    {
                        startReturnTime = Time.fixedTime;
                    }

                    nowAngleTheta = fixedFloatLerp(nowAngleTheta, angleTheta, startTime, lerpBackTime);
                    LookAt2D(transform.position, transform.position + new Vector3(Mathf.Cos((nowAngleTheta - 90f) * Mathf.Deg2Rad), Mathf.Sin((nowAngleTheta - 90f) * Mathf.Deg2Rad), 0f), startReturnTime, lerpReturnTime);
                }
            }

            if (nowAngleTheta == angleTheta)
            {
                nowMove = true;
                nowStare = true;
                wasStare = true;
                backStare = true;
            }
        }
        else if (wasStare && nowStare && fow.visibleTarget.Count != 0)
        {
            nowStare = false;
            startStareTime = Time.fixedTime;
        }
        else if (wasStare && !nowStare && fow.visibleTarget.Count != 0)
        {
            LookAt2D(transform.position, fow.visibleTarget[0].position, startStareTime, lerpStareTime);
            nowAngleTheta = Vector2.SignedAngle(new Vector2(0, 1), transform.position - fow.visibleTarget[0].position);
        }
        else if (!wasStare && fow.visibleTarget.Count != 0)
        {
            if (backStare)
            {
                backStare = false;
                startStareTime = Time.fixedTime;
            }
            else
            {
                LookAt2D(transform.position, fow.visibleTarget[0].position, startStareTime, lerpStareTime);
                nowAngleTheta = Vector2.SignedAngle(new Vector2(0, 1), transform.position - fow.visibleTarget[0].position);
            }
        }

        Debug.Log(angleTheta);
    }

    #region Methods
    // LookAt function with lerp in 2-D world.
    private void LookAt2D(Vector2 positionA, Vector2 positionB, float timeStartLerp, float lerpTime)
    {
        Vector3 angleAB = new Vector3(0f, 0f, Vector2.SignedAngle(transform.right, positionB - positionA));
        transform.eulerAngles = fixedVector3Lerp(transform.eulerAngles, transform.eulerAngles + angleAB, timeStartLerp, lerpTime);
    }

    private Vector3 fixedVector3Lerp(Vector3 start, Vector3 end, float timeStartLerp, float lerpTime)
    {
        Vector3 result = new Vector3();

        float timeSinceStart = Time.fixedTime - timeStartLerp;
        float percentageComplete = timeSinceStart / lerpTime;

        result.x = Mathf.Lerp(start.x, end.x, percentageComplete);
        result.y = Mathf.Lerp(start.y, end.y, percentageComplete);
        result.z = Mathf.Lerp(start.z, end.z, percentageComplete);

        return result;
    }

    private float fixedFloatLerp(float start, float end, float timeStartLerp, float lerpTime)
    {
        float timeSinceStart = Time.fixedTime - timeStartLerp;
        float percentageComplete = timeSinceStart / lerpTime;

        var result = Mathf.Lerp(start, end, percentageComplete);

        return result;
    }

    // Can use only on 2D. Set vector1 as basical axis.
    //private float AngleBetweenTwoVectors(Vector3 vector1, Vector3 vector2) 
    //{
    //    vector1.normalized

    //    return;
    //}
    #endregion
}
