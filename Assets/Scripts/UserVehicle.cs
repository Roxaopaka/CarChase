using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class UserVehicle : MonoBehaviour
{
     
     private Rigidbody rb;
     private float movementX;
     private float movementY;
     
     private float newXValue;
     private Vector2 move2d;

     private float speed;

     private float maxSpeed;
     private float minSpeed;
     private float frictionConstant;
     private float rotateSpeed;

     private float currentXRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 0;
        rb = GetComponent<Rigidbody>();
        rotateSpeed = 90;
        maxSpeed = 70;
        minSpeed = -40;
        frictionConstant = 5;
        rb.mass = 1;
    }

    // Update is called once per frame
    void Update()
    {

        // Pressing W and Accelerating
        if(movementY > 0 && speed < maxSpeed)
        {
            speed +=15*Time.deltaTime;
            Debug.Log("RUNN");
        }

        // Pressing S and Decelerating
        if(movementY < 0 && speed > minSpeed)
        {
            speed -=30*Time.deltaTime;
        }

        //Make sure speed is not lower than minimum speed
        if (speed < minSpeed)
        {
            speed = minSpeed;
        }

        //If player does not want to acc or dec, let friction slow the car
        if(movementY == 0 && speed>0)
        {
            speed-=frictionConstant*Time.deltaTime;
            if (speed < 0)
            {
                speed = 0;
            }
        }

        //If player does not want to acc or dec, let friction slow the car
        if(movementY == 0 && speed < 0)
        {
            speed+=frictionConstant*Time.deltaTime;
            if (speed > 0)
            {
                speed = 0;
            }
        }
        
        //If player is pressing A, rotate left
        if(movementX < 0)
        {
            transform.Rotate(0,rotateSpeed*-1*Time.deltaTime,0);
            speed=speed-rotateSpeed*0.2f*Time.deltaTime;
        }

        //If player is pressing D, rotate right
        if(movementX > 0)
        {
            transform.Rotate(0,rotateSpeed*1*Time.deltaTime,0);
            speed=speed-rotateSpeed*0.2f*Time.deltaTime;
        }
        

    }

    void FixedUpdate()
    {
        Vector3 movement = (transform.forward) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
        Vector3 currentEulerAngles = transform.eulerAngles;
        // Modify the desired axes
        currentEulerAngles.x = newXValue; // Set X to a specific value
        currentEulerAngles.z = 0f;         // Set Z to 0
        // Reassign the whole vector back
        transform.eulerAngles = currentEulerAngles;        
        }

    void OnMove(InputValue inputV)
    {
        move2d = inputV.Get<Vector2>();

        
            movementX = move2d.x;
            movementY = move2d.y;
        
    }

    
}


