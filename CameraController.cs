using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Axis color: x is red
               y is green
               z is blue.
*/

public class CameraController : MonoBehaviour
{
    private float moveSpeed = 6f;
    
    private Rigidbody2D rb;
    private Camera camSight;
    private Vector2 ChaVelocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        camSight = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mosPosition = camSight.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        LookAt2D(transform.position, mosPosition, 100f);

        ChaVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        //rb.MovePosition(rb.position + ChaVelocity * Time.fixedDeltaTime);
        rb.velocity = ChaVelocity;
    }

    // LookAt function with lerp in 2-D world.
    private void LookAt2D(Vector2 positionA, Vector2 postionB, float lerpSpeed)
    { 
        Vector3 angleAB = new Vector3(0f, 0f, Vector2.SignedAngle(-transform.right, positionA - postionB));
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, transform.eulerAngles + angleAB, lerpSpeed * Time.deltaTime);
    }
}
