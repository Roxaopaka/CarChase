using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class CarDriveMechs : MonoBehaviour //<-- This is the script for the Cop Car Pre Fab
{
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;   //Create a public reference so that the component, "navMeshAgent" can be used in the code
    private Vector3[] targetPoints;
    private Vector3 patrolLocation;
    private float hitTimer = 0; //This timer is to check if the cop car is in contact with the user's car for 4 seconds. If hitTimer = 4, game over
    private float maxSpeed = 50;
    public GameObjManager boss;
    public float speed;

    public bool userFound;
    public Vector3 lastKnownLocation;

    private float fieldOfViewAngle = 270;

    public LayerMask userLayer;
    public LayerMask wallLayer;

    public String state = "";

    private bool currentlySearchingLocation = false;

    private int maxViewDistance = 100;
    
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        
        rb = GetComponent<Rigidbody>();
        // Prevent Rigidbody from fighting the NavMeshAgent
        rb.isKinematic = true;
        
    }
    void Start()
    {
        Collider meshCollider = boss.getMeshCollider();
        Bounds b = meshCollider.bounds;

        // Points on the cop car used for line-of-sight raycasts.
        targetPoints = new Vector3[]
        {
            b.center,
            new Vector3(b.center.x, b.center.y, b.max.z),
            new Vector3(b.center.x, b.center.y, b.min.z),
            new Vector3(b.min.x, b.center.y, b.center.z),
            new Vector3(b.max.x, b.center.y, b.center.z),
        };
    }

    // Update is called once per frame
    void Update()
    {
        Collider meshCollider = boss.getMeshCollider();
        Bounds b = meshCollider.bounds;
        targetPoints = new Vector3[]
        {
            b.center,
            new Vector3(b.center.x, b.center.y, b.max.z),
            new Vector3(b.center.x, b.center.y, b.min.z),
            new Vector3(b.min.x, b.center.y, b.center.z),
            new Vector3(b.max.x, b.center.y, b.center.z),
        };

        canIseePlayer();
        //Keep the car at a constant height (0.5f)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z);

        //The angular speed decreases as linear speed increases, which simulates real world physics
            navMeshAgent.angularSpeed = 400-navMeshAgent.velocity.magnitude;
            //If the user is still not caught
            if (boss.getCaught() != true && boss.getUserFound()==true)
            {   //Set the destination of the navMeshAgent to the user's location
                navMeshAgent.SetDestination(boss.getUserLocation());
                state = "";
                
            }else{

            if (state.Equals("LookLastKnown") && currentlySearchingLocation==false)
            {
                searchLastKnown();
            }
            if(state.Equals("LookLastKnown") && currentlySearchingLocation == true && Vector3.Distance(transform.position, boss.getLastKnownLocation()) < 1.5f)
            {
                currentlySearchingLocation = false;
                state = "";
            }
            if(state.Equals("Patrolling") && (Vector3.Distance(transform.position, patrolLocation) < 1.5f))
            {
                state = "";
            }

    

        
    }
        
        speed = navMeshAgent.velocity.magnitude;
        
    }


    //This function makes sure that the cop car doesn't just ram through the props without losing speed (ie. lamps, benches)
    void OnCollisionEnter(Collision collision)
    {
        //If the cop car hits a prop, set the max speed to 50% UNTIL the cop car escapes the prop
        if (collision.gameObject.CompareTag("Props"))
        {
            navMeshAgent.speed = maxSpeed*0.5f;
        }

        if (collision.gameObject.CompareTag("PushBack"))
        {
            navMeshAgent.speed = navMeshAgent.speed*-0.2f;
        }
}


    //This function counts how many seconds the cop car is in consecutive contact with the user car
    void OnCollisionStay(Collision collision)
    {
        //If the cop car hits the user car
        if (collision.gameObject.CompareTag("CAR"))
        {
            //Start counting up
            hitTimer+=1*Time.deltaTime;
            //If it gets to 4 seconds
            if (hitTimer >= 4)
            {
                //The cop car has arrested the user car 
                Destroy(collision.gameObject);
            }
        }
    }

    //This function is called when the cop car stops colliding with another object
    void OnCollisionExit(Collision collision)
    {
        //If the cop car stops hitting the user car, reset the timer
        if (collision.gameObject.CompareTag("CAR"))
        {
            hitTimer = 0f;
        }
        //If the cop car stops hitting the prop car, restore the max speed
        if (collision.gameObject.CompareTag("Props"))
        {
            navMeshAgent.speed = maxSpeed;
        }
    }


    public float getHitTimerInitial()
    {
        return hitTimer;
    }

    public float getSpeedInitial()
    {
        return speed;
    }

    public bool getUserFoundInitial()
    {
        return userFound;
    }

    public Vector3 getLastKnownLocationInitial()
    {
        return lastKnownLocation;
    }

    private void canIseePlayer()
    {
        if (Vector3.Distance(boss.getUserLocation(), transform.position) < maxViewDistance)
        {   
            
            Vector3 toUserVector = boss.getUserLocation() - transform.position;
            float angle = Vector3.Angle(transform.forward, toUserVector);
            if (angle < fieldOfViewAngle/2f)
            {
                
                if(raysAttack()){
                   userFound = true;
            }
            else
            {
                userFound = false;
            }  

        }
        else
        {
            userFound = false;
        }
    }else
        {
            userFound = false;
        }

    }


    private bool raysAttack()
    {
        for(int i = 0; i < targetPoints.Length;i++){
            if(Physics.Raycast(transform.position,targetPoints[i]-transform.position,out RaycastHit hit,maxViewDistance)){
                if(hit.collider.CompareTag("CAR")){
                    return true;
                }
            }
        }

        return false;
    }

    private void searchLastKnown()
    {
        navMeshAgent.SetDestination(boss.getLastKnownLocation());
        currentlySearchingLocation = true;
    }

    public void patrolling(Vector3 location)
    {
        patrolLocation = location;
        navMeshAgent.SetDestination(location);
    }


    public void setState(String s)
    {
        state = s;
        Debug.Log(state);
    }

    public String getState()
    {
        return state;
    }


    
}
