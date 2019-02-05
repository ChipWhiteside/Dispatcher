using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeMovement : MonoBehaviour {

    public float speed;

    private Rigidbody rb;
    private bool startRotatingLeft;
    private bool startRotatingRight;
    private bool forward;
    private bool backward;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void Update()
    {
        if (startRotatingLeft)
        {
            // Rotate the object around its local X axis at 25 degrees per second
            transform.Rotate(Vector3.down * 75 *Time.deltaTime);

            // ...also rotate around the World's Y axis
            //transform.Rotate(Vector3.down * 25 * Time.deltaTime, Space.World);
        }
        if (startRotatingRight)
        {
            // Rotate the object around its local X axis at 25 degrees per second
            transform.Rotate(Vector3.up * 75 * Time.deltaTime);

            // ...also rotate around the World's Y axis
            //transform.Rotate(Vector3.up * 25 * Time.deltaTime, Space.World);
        }
        if (forward) {
            transform.Translate(Vector3.forward * 50 * Time.deltaTime);
        }
        if (backward)
        {
            transform.Translate(Vector3.back * 50 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            startRotatingLeft = true;
        if (Input.GetKeyUp(KeyCode.LeftArrow)) 
            startRotatingLeft = false;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            startRotatingRight = true;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            startRotatingRight = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            forward = true;
        if (Input.GetKeyUp(KeyCode.UpArrow))
            forward = false;

        if (Input.GetKeyDown(KeyCode.DownArrow))
            backward = true;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            backward = false;
    }
}
