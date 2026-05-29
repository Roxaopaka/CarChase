using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class CarDriveMechs : MonoBehaviour //<-- This is the script for the Cop Car Pre Fab
{
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;   //Create a public reference so that the component, "navMeshAgent" can be used in the code
    public float chaseTurnSpeed = 720f;
    public float patrolTurnSpeed = 540f;

    //Target points is the the raycast's target location (ie. the user car's collider boundaries)
    private Vector3[] targetPoints;
    //PatrolLocation stores the random location that the cop car must look, which is assigned by the Spawner
    private Vector3 patrolLocation;
    private float hitTimer = 0; //This timer is to check if the cop car is in contact with the user's car for 4 seconds. If hitTimer = 4, game over
    private float maxSpeed = 50;
    public GameObjManager boss;
    public float speed;

    //Each cop car requires userFound so that the Boss can see how many cop cars see the user. If its more or equal to 1,
    //all cop cars get the user location
    public bool userFound; 
    //This stores the lastKnownLocation that THIS SPECIFIC COP CAR saw the user
    public Vector3 lastKnownLocation;

    //This fieldOfViewAngle is how wide. the cop car can see. The wider the FOV, the more area around the cop car that the cop can see
    //This fieldOfViewAngle is the second barrier to start sending rays to the user (canIseePlayer())
    private float fieldOfViewAngle = 270;

    //All walls exist on the wallLayer. The wallLayer is used so that raycast knows which object to recognize as an obstruction
    //Only objects existing on the wallLayer are considered to be an obstruction for the raycast
    public LayerMask wallLayer;
    
    //The state is essentially the current task of a cop car. There are three potential states:
    // 1: "" <-- This means that the cop car is either jobless (looking to be assigned Patrolling job) or it is chasing the user car
    //For "", if it is jobless, there is only a split second before it is assigned a role, as Spawner is constantly checking for cops that are jobless (in update())
    // 2: "LookLastKnown" <-- This means that the cop car is assigned to check the lastKnown user location
    // 3: "Patrolling" <-- This means that the cop car is assigned the patrolling job
    public String state = "";

    private bool currentlySearchingLocation = false;

    //MaxViewDistance is the maximum distance that the cop car can see
    //This is the first barrier to start sending rays to the user (canIseePlayer())
    private int maxViewDistance = 200;
    
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); 
        
        rb = GetComponent<Rigidbody>();
        // Prevent Rigidbody from fighting the NavMeshAgent
        rb.isKinematic = true;
        navMeshAgent.autoBraking = false;
        navMeshAgent.stoppingDistance = 0f;
        
    }
    void Start()
    {
        //Updates the user's collider's boundaries so that raycast will be sent in the right direction
        updateTargetPoints();
    }

    // Update is called once per frame
    void Update()
    {
        //If the cop car is either patrolling or looking last known, don't speed. Go slow and steady. 
        if(state.Equals("Patrolling") || state.Equals("LookLastKnown"))
        {
            navMeshAgent.speed = 20f;
            navMeshAgent.acceleration = 10f;
            navMeshAgent.angularSpeed = patrolTurnSpeed;
        }
        //If the cop car is currently chasing the user, increase max speed and acceleration
        else
        {
            navMeshAgent.speed = 42;
            navMeshAgent.acceleration = 25f;
            navMeshAgent.angularSpeed = chaseTurnSpeed;
        }
        //Update the user's collider's boundaries
        updateTargetPoints();

        canIseePlayer();
        //Keep the car at a constant height (0.5f)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z);

            //If the user is still not caught and user is seen by a cop
            if (boss.getCaught() != true && boss.getUserFound()==true)
            {   //Set the destination of the navMeshAgent to the user's location
                navMeshAgent.SetDestination(boss.getUserLocation());
                state = "";
                currentlySearchingLocation = false;

                
            }else{
            //If it is assigned "LookLastKnown" by spawner and it hasn't already started the job, call searchLastKnown()
            if (state.Equals("LookLastKnown") && currentlySearchingLocation==false)
            {
                searchLastKnown();
            }
            //If it is assigned "LookLastKnown" and it got to the last seen location, become jobless
            if(state.Equals("LookLastKnown") && currentlySearchingLocation == true && Vector3.Distance(transform.position, boss.getLastKnownLocation()) < 1.5f)
            {
                currentlySearchingLocation = false;
                state = "";
            }
            //If it is patrolling and it got to the patrol location, become jobless
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
        if (isUserVehicle(collision.gameObject))
        {
            //Start counting up
            hitTimer+=1*Time.deltaTime;
            //If it gets to 4 seconds
            if (hitTimer >= 4)
            {
                //The cop car has arrested the user car 
                Destroy(boss.UserVehicle);
            }
        }
    }

    //This function is called when the cop car stops colliding with another object
    void OnCollisionExit(Collision collision)
    {
        //If the cop car stops hitting the user car, reset the timer
        if (isUserVehicle(collision.gameObject))
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

    //THIS IS THE INTERESTING PART
    //canIseePlayer checks if the cop car has a clear line of sight of the user
    //Two conditions must be met for a ray to be sent
    //First, the delta distance is less than the max view distance
    //Second, the user car is in the cop cars FOV
    //These two conditions must be met for a ray to be sent. 
    //By having two conditions, it reduces the chance for the ray to be sent unless it is CONFIDENT that it will hit the user
    //Less rays sent = Less lag
    private void canIseePlayer()
    {
        //Is the delta distance less than max distance?
        if (Vector3.Distance(boss.getUserLocation(), transform.position) < maxViewDistance)
        {   
            
            Vector3 toUserVector = boss.getUserLocation() - transform.position;
            float angle = Vector3.Angle(transform.forward, toUserVector);
            //Is the car within FOV?
            if (angle < fieldOfViewAngle/2f)
            {
                //If both conditions pass, lets start shooting rays (check out raysAttack())
                if(raysAttack()){
                    //If raysAttack() is true, user is found
                   userFound = true;
                   lastKnownLocation = boss.getUserLocation();
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


    //This function is the one that actually sends rays. 
    private bool raysAttack()
    {
        //for every boundary in the user's collider,
        for(int i = 0; i < targetPoints.Length;i++){
            //shoot a ray towards it and objects on the wall layer are obstructions
            if(!Physics.Linecast(transform.position, targetPoints[i], wallLayer)){
                //If it hits, return true
                return true;
            }
        }

        return false;
    }

    private bool isUserVehicle(GameObject hitObject)
    {
        if (boss != null && boss.UserVehicle != null)
        {
            Transform userVehicleTransform = boss.UserVehicle.transform;
            return hitObject == boss.UserVehicle || hitObject.transform.IsChildOf(userVehicleTransform);
        }

        return hitObject.CompareTag("CAR") || hitObject.transform.root.CompareTag("CAR");
    }

    //Update the cop car's boundary
    private void updateTargetPoints()
    {
        Bounds b = boss.getUserBounds();

        // Points on the user's car used for line-of-sight checks.
        targetPoints = new Vector3[]
        {
            b.center,
            new Vector3(b.center.x, b.center.y, b.max.z),
            new Vector3(b.center.x, b.center.y, b.min.z),
            new Vector3(b.min.x, b.center.y, b.center.z),
            new Vector3(b.max.x, b.center.y, b.center.z),
        };
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
    }

    public String getState()
    {
        return state;
    }


    
}
