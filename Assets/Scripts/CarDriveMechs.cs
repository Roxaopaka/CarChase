using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
public class CarDriveMechs : MonoBehaviour //<-- This is the script for the Cop Car Pre Fab
{
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;   //Create a public reference so that the component, "navMeshAgent" can be used in the code
    private float hitTimer = 0; //This timer is to check if the cop car is in contact with the user's car for 4 seconds. If hitTimer = 4, game over
    private bool hit = false; //If the cop car catches the user, this becomes true
    private float maxSpeed = 50;
    public GameObjManager boss;
    
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        
        rb = GetComponent<Rigidbody>();
        // Prevent Rigidbody from fighting the NavMeshAgent
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Keep the car at a constant height (0.5f)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z);

        //The angular speed decreases as linear speed increases, which simulates real world physics
            navMeshAgent.angularSpeed = 400-navMeshAgent.velocity.magnitude;
            //If the user is still not caught
            if (hit != true)
            {   //Set the destination of the navMeshAgent to the user's location
                navMeshAgent.SetDestination(boss.getUserLocation());
                
            }else{
                navMeshAgent.ResetPath();
    }   
        
        
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
                hit = true;
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

    public bool getHitInitial()
    {
        return hit;
    }
    
        
    

}
