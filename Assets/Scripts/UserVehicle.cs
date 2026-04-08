using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading;
using System.Runtime.CompilerServices;

public class UserVehicle : MonoBehaviour
{
     
     private Rigidbody rb;
     private float count;

     public int wantedLevel;

     private float movementX;
     private float movementY;
     
     private float newXValue;
     private Vector2 move2d;

     private float speed;

     private float maxSpeed;
     private float minSpeed;
     private float frictionConstant;
     private float rotateSpeed;

     private int iteration;

     private float currentXRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 0;
        rb = GetComponent<Rigidbody>();
        rotateSpeed = 90;
        maxSpeed = 50;
        minSpeed = -20;
        frictionConstant = 5;
        rb.mass = 1;


        //*THE FOLLOWING 3 LINES WERE ASSISTED BY AI (CLAUDE)
        rb.constraints = RigidbodyConstraints.FreezePositionY 
               | RigidbodyConstraints.FreezeRotationX 
               | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 30)
        {
            wantedLevel=1;
        }
        // Pressing W and Accelerating
        if(movementY > 0 && speed < maxSpeed)
        {
            speed +=15*Time.deltaTime;
        }

        // Pressing S and Decelerating
        if(movementY < 0 && speed > 0)
        {
            speed -=30*Time.deltaTime;
        }

        if(movementY<0 && speed>minSpeed && speed < 0)
        {
            speed-=10*Time.deltaTime;
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

    void FixedUpdate(){
    
    
    //*THE FOLLOWING 3 LINES WERE ASSISTED BY AI (CLAUDE)
        // Prevent collision impulses from persisting — we handle movement manually
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
    rb.MovePosition(rb.position + movement);

        if (wantedLevel > 0)
        {
            count+=Time.deltaTime;
            if (count > 60)
            {
                wantedLevel++;
            }
        }
    
    }
    

    void OnMove(InputValue inputV)
    {
        move2d = inputV.Get<Vector2>();

        
            movementX = move2d.x;
            movementY = move2d.y;
        
    }

    public int getIteration()
    {
        return iteration;
    }

    public void OnTestKey()
    {
        iteration++;
    }

    public int getWantedLevel()
    {
        return wantedLevel;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PushBack"))
        {
            speed = speed*0.2f;
        }
    }
    



    
}


