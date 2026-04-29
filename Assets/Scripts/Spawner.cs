using UnityEngine;
using System.Collections.Generic;
using System;

public class Spawner : MonoBehaviour //This is the script for the spawner of cop car objects
{

    public GameObject copPreFab; //Reference for the cop car prefab

    private int currentWantedLevel; //Store the current wanted level

    private int numCars; //Store the number of police cars
    public GameObjManager boss;

    private GameObject closestCop = null;

    public List<GameObject> allCops;

    public List<GameObject> roadObjects;

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


        if (boss.getUserFound() == false)
        {
            if (alreadyLookingLastKnown == false)
            {
                searchLastKnownLocation();
            }
            else
            {
                startPatrolling();
            }
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

    public void startPatrolling()
    {
        List<Vector3> checkLocations = new List<Vector3>();
        foreach(GameObject cop in allCops)
        {
            CarDriveMechs copScript = cop.GetComponent<CarDriveMechs>();
            if (copScript.getState() == "")
            {
                bool found = false;
                while (found == false)
                {
                    Vector3 location = generateRandomCoordinate(roadObjects);
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
                Debug.Log(checkLocations[checkLocations.Count-1]);
                copScript.setState("Patrolling");
                copScript.patrolling(checkLocations[checkLocations.Count-1]);
                
            }
        }
        
    }

    public Vector3 generateRandomCoordinate(List<GameObject> theseObjects)
    {
        
        Vector3 location = new Vector3();
        int random = UnityEngine.Random.Range(0,theseObjects.Count);
        location = theseObjects[random].transform.position;
            
        
        return location;
    }

    

}