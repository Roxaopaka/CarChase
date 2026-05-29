using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

public class Spawner : MonoBehaviour //This is the script for the spawner of cop car objects
{

    public GameObject copPreFab; //Reference for the cop car prefab

    public bool droneSpawned = false;


    private int currentWantedLevel; //Store the current wanted level

    private int numCars; //Store the number of police cars
    public GameObjManager boss;

    public GameObject drone;

    //The reference to the closestCop to the user's location
    //This is used to assign who checks the last known location 
    //The closest car checks the last known location
    private GameObject closestCop = null;

    //Array of all cop objects (child objects to the spawner)
    public List<GameObject> allCops;

    //Array of roadObjects collected from the Boss
    //This is used to assign patrol locations to cops by picking a random roadObject's location
    //The patrol location must be a road because it doesn't make sense for cop cars to patrol like a building
    public List<GameObject> roadObjects;
    //AlreadyLookingLastKnown stores if a cop is already looking the last known location. 
    //This stops multiple cars from being assigend to go to the last known location
    public bool alreadyLookingLastKnown = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roadObjects = boss.findGameObjectsInLayer(7);
    }

    //Fixed update does not run every frame. Instead, it runs every 0.02 seconds
    void FixedUpdate()
    {

        //This if statement is to check if the user's wanted level went up
        if (currentWantedLevel - boss.getWantedLevel() <0)
        {
            //Instantiate the prefab
            GameObject copSpawn = Instantiate(copPreFab,this.transform);
            CarDriveMechs script = copSpawn.GetComponent<CarDriveMechs>();
            script.boss = boss;
            numCars++;
            allCops.Add(copSpawn);
        }
        currentWantedLevel = boss.getWantedLevel();
        if (currentWantedLevel>1 && droneSpawned == false)
        {
            droneSpawned = true;
            Vector3 spawnLocation = new Vector3(-677.4f,103,-38.00942f);

            Debug.Log("SPAWNED");
            GameObject droneSpawn = Instantiate(drone, this.transform);
            DroneController droneScript = droneSpawn.GetComponent<DroneController>();
            droneScript.boss = boss;
        }

        //THIS IS THE INTERESTING PART
        //If the user isn't found, the spawner has to assign one cop to check the last known location,
        //and all other cop cars to start patrolling (ie. check around the map for the user)
        if (boss.getUserFound() == false)
        {
            //If no cop has started to look for the last known location, call searchLastKnownLocation()
            if (alreadyLookingLastKnown == false)
            {
                searchLastKnownLocation();
            }
            //If one cop already is searching the last known location, call startPatrolling()
            else
            {
                startPatrolling();
            }
        }
        //If one cop sees the user, cancel the order to check the last known location, and setAreaCost for grass and non-road
        //terrain to 1 (same as the road). The reason why the code changes the cost for terrain is because
        //I don't want the cop to be driving on the grass and non-road terrain when patrolling,
        //so when the cop Car is patrolling, the cost for driving on those non-road terrain is 1000 (basically saying don't ever go on it)
        //But when the user is found, now its time to chase, so the the cost for those non-road terrain becomes 1 (basically drive wherever you want to drive)
        if (boss.getUserFound() == true)
        {
            Debug.Log("User Found");
            alreadyLookingLastKnown = false;
            NavMesh.SetAreaCost(0,1);
        }
}
    //Getter method to return the number of police cars on the map
    public int getNumCarsInitial()
    {
        return numCars;
    }

    public List<GameObject> getInitialAllCops()
    {
        return allCops;
    }

    public bool getDroneSpawnedInitial()
    {
        return droneSpawned;
    }

    //This function assigns the closest cop to the last known user location to check the last known user location
    //It does this by searting the minimum delta distance between each cop car and the user location
    //The cop car with the minimum delta distance is assigend to state "LookLastKnown", which orders it to check out the last known location
    public void searchLastKnownLocation()
    {
        
        if (allCops == null || allCops.Count == 0)
        {
            return;
        } 
        if(alreadyLookingLastKnown!=true){
        float currentDistance;
        float min = 100000000f;
        foreach(GameObject cops in allCops)
        {
            currentDistance = Vector3.Distance(boss.getUserLocation(), cops.transform.position);
            if (currentDistance < min)
            {
                min = currentDistance;
                closestCop = cops;
            }
        }

        CarDriveMechs closestCopScript = closestCop.GetComponent<CarDriveMechs>();
        if (closestCopScript != null)
        {
            closestCopScript.setState("LookLastKnown");
            alreadyLookingLastKnown = true;
        }
       
    }
    }

    //StartPatrolling is a function that makes all cop cars other than the one ordered to check lastKnownLocation to start patrolling
    //It does this by assigning a random target location for each cop car.
    //The random target is chosen from an array of location of road objects. It also has a duplication check so that no cop car is assigned to the same road tile
    public void startPatrolling()
    {
        List<Vector3> checkLocations = new List<Vector3>();
        foreach(GameObject cop in allCops)
        {
            CarDriveMechs copScript = cop.GetComponent<CarDriveMechs>();
            //CopCars who have state "" basically are jobless. They aren't assigned any task, so they are ordered to go on patrol
            if (copScript.getState() == "")
            {
                bool found = false;
                while (found == false)
                {
                    //while found is false, generate a random coordinate from road objects
                    //the for loop checks if there is no duplicate
                    //if there is no duplicate, it adds the assigned coordinate to checkLocations[] and found becomes true
                    Vector3 location = generateCoordinate(roadObjects);
                    int equal = 0;
                    for(int z = 0; z < checkLocations.Count; z++)
                    {
                        if (checkLocations[z] == location)
                        {
                            equal++;
                        }
                    }
                    if (equal == 0)
                    {
                        found = true;
                        checkLocations.Add(location);
                    }
                }
                copScript.setState("Patrolling");
                copScript.patrolling(checkLocations[checkLocations.Count-1]);
                //The area cost of non-road terrain becomes 1000, so the cop car MUST take the road to get to that road tile
                NavMesh.SetAreaCost(0, 1000);
                
            }
        }
        
    }
    //This function generates a random coordinate from a list of gameobjects
    public Vector3 generateCoordinate(List<GameObject> theseObjects)
    {
        
        Vector3 location = new Vector3();
        int random = UnityEngine.Random.Range(0,theseObjects.Count);
        location = theseObjects[random].transform.position;
            
        
        return location;
    }

    

}
