using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserVehicle : MonoBehaviour
{
     
     private Rigidbody rb;
     private float count;

     public int wantedLevel;

     private float movementX;
     private float movementY;
     
     private Vector2 move2d;

     private float speed;

     private float maxSpeed;
     private float minSpeed;
     private float frictionConstant;
     private float rotateSpeed;

     private int iteration;
     private float health;
    public GameObjManager boss;

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
        health = 100f;


        //*THE FOLLOWING 3 LINES WERE ASSISTED BY AI (CLAUDE)
        rb.constraints = RigidbodyConstraints.FreezePositionY 
               | RigidbodyConstraints.FreezeRotationX 
               | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        //If the user car breaks the speed limit, set the wanted level to 1
        if (speed > 30 && wantedLevel==0)
        {
            wantedLevel=1;
        }
        // Pressing W and Accelerating
        if(movementY > 0 && speed < maxSpeed)
        {
            speed +=15*Time.deltaTime;
        }

        // Pressing S and Decelerating <-- THIS IS BRAKING
        if(movementY < 0 && speed > 0)
        {
            speed -=30*Time.deltaTime;
        }
        // Pressing S and Decelerating <-- THIS IS REVERSE
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

        if (wantedLevel > 0)
        {
            count+=Time.deltaTime;
            if (count > 10)
            {
                wantedLevel++;
                Debug.Log(wantedLevel);
                count-=10;
            }
        }

        if (boss.getCaught() == true)
        {
            Destroy(gameObject);
        }

    }

    void FixedUpdate(){
    
    
    //*THE FOLLOWING 4 LINES WERE ASSISTED BY AI (CLAUDE)
        // Prevent collision impulses from persisting — we handle movement manually
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
    rb.MovePosition(rb.position + movement);

    }
    

    void OnMove(InputValue inputV)
    {
        move2d = inputV.Get<Vector2>();

        
            movementX = move2d.x;
            movementY = move2d.y;
        
    }


    public void OnTestKey()
    {
        if (iteration == 0)
        {
            iteration=1;
        }
        else
        {
            iteration=0;
        }
    }



    //If the user hits a building, get pushed back by reversing the speed
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PushBack"))
        {
            speed = speed*-0.2f;
        }
        if (collision.gameObject.CompareTag("COP"))
        {
            CarDriveMechs script = collision.gameObject.GetComponent<CarDriveMechs>();
            health = health-Math.Abs(speed-script.getSpeedInitial())*0.9f;
            Debug.Log(health + "HEALTH");
            if (health < 0)
            {
                Destroy(gameObject);
                
            }
        }
    }

    public int getIterationInitial()
    {
        return iteration;
    }

    public int getWantedLevelInitial()
    {
        return wantedLevel;
    }

    public float getSpeedInitial()
    {
        return speed;
    }

    public Vector3 getUserLoccationInitial()
    {
        return transform.position;
    }

    public float getHealthInitial()
    {
        return health;
    }



    



    
}


