using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class UserVehicle : MonoBehaviour
{
     
     private Rigidbody rb;
     private float movementX;
     private float movementY;
     
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
        maxSpeed = 40;
        minSpeed = -20;
        frictionConstant = 5;
    }

    // Update is called once per frame
    void Update()
    {
        print(speed);

        if(transform.position.y<5){
        // Pressing W and Accelerating
        if(movementY > 0 && speed < maxSpeed)
        {
            speed +=15*Time.deltaTime;
        }

        // Pressing S and Decelerating
        if(movementY < 0 && speed > minSpeed)
        {
            speed -=10*Time.deltaTime;
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
            speed=Math.Abs(speed-rotateSpeed*0.2f*Time.deltaTime);
        }

        //If player is pressing D, rotate right
        if(movementX > 0)
        {
            transform.Rotate(0,rotateSpeed*1*Time.deltaTime,0);
            speed=Math.Abs(speed-rotateSpeed*0.2f*Time.deltaTime);
        }

        }
        

    }

    void FixedUpdate()
    {
        Vector3 movement = (transform.forward) * speed * Time.fixedDeltaTime;
        print(movement);
        rb.MovePosition(rb.position + movement);
    }

    void OnMove(InputValue inputV)
    {
        move2d = inputV.Get<Vector2>();

        
            movementX = move2d.x;
            movementY = move2d.y;
        
    }
}


