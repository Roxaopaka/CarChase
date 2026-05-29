using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameObjManager : MonoBehaviour
{
    //THIS IS THE BIG BOSS
    // Basically the big boss is a big storage of references for all gameobjects and each variable in a gameobject
    //By making a single place have everything, it makes it so that other game objects don't have to reference like the 4+ other gameobjects and call it in its script
    //Just by having a reference to the big boss, the gameobject gets access to everything
    //It also stores universal functions that might be useful for multiple gameobjects, such as findGameObjectOnLayer()
    public GameObject UserVehicle; //Reference for the user car
    public GameObject Camera; //Ref to the camera
    public GameObject CopCar; //Ref to the cop car
    public GameObject CopCarSpawner; //Ref to the cop car spawner

    public PrometeoCarController UserVehicleScript; //Script for user car (PrometeoCarController)
    public cameraMovement CameraScript; //Script for camera (cameraMovement)
    public CarDriveMechs CopCarScript; //Script for cop car prefab (CarDriveMechs)
    public Spawner CopCarSpawnerScript; //Script for copCar spawner (Spawner)
    public bool caught=false; //Bool that stores if the user gets caught
    //There are two ways for the user car to get caught
    // 1: The cop car is in contact with the user car CONSECUTIVELY for 4 seconds
    // 2: The cop car destroys the user car by ramming into it with a big enough speed difference between user car and cop car

    private bool isUserFound; //Bool that stores if the user is found
    //If the user is found (seen), ALL cop car receives the location of the user
    private Vector3 lastKnownLocation; //Vector 3 that stores the location of the user car's last seen location
    //The closest cop car to the last seen location of the user will check that location

    private int numCarsSee; //Integer that stores how many cop cars can see the user

    public GameObject[] findOnLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //The following 5 lines just get the script values for each gameobject
        UserVehicleScript = UserVehicle.GetComponent<PrometeoCarController>(); 
        CameraScript = Camera.GetComponent<cameraMovement>();
        CopCarScript = CopCar.GetComponent<CarDriveMechs>();
        CopCarSpawnerScript = CopCarSpawner.GetComponent<Spawner>();
        numCarsSee = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (CopCarSpawnerScript.getDroneSpawnedInitial() == true)
        {
            isUserFound = true;
        }
        //THIS IS THE IMPORTANT PART
        //This if statement updates caught if the gameobject is destroyed
        if (UserVehicle == null)
        {
            caught = true;
        }
        //GetAllCops returns an array of gameobjects that contain all child objects of copcar spawner
        List<GameObject> allCops = getAllCops();

        //This for loop checks to see if even a single cop car sees the user. 
        //If even one cop sees the user, isUserFound becomes true and then all cop gets access to the user location
        //This for loop works by checking each cops status on whether it sees the user. If one user sees the cop,
        //Lask Known location becomes updated and all cop gets access to the user location
        for(int i = 0; i < allCops.Count; i++)
        {
            CarDriveMechs copScript = allCops[i].GetComponent<CarDriveMechs>();
            if (copScript.getUserFoundInitial()==true)
            {
                numCarsSee++;
                lastKnownLocation = copScript.getLastKnownLocationInitial();
            }
        }

        if (numCarsSee >= 1)
        {
            isUserFound = true;
        }
        else
        {
            isUserFound = false;
        }
    
        numCarsSee = 0;
        
    

    }


//USER VEHICLE GETTERS
    public int getWantedLevel()
    {
        return UserVehicleScript.getWantedLevelInitial();
    }

    public int getIteration()
    {
        return UserVehicleScript.getIterationInitial();
    }

    public float getSpeed()
    {
        return UserVehicleScript.getSpeedInitial();
    }

    public Vector3 getUserLocation()
    {
        return UserVehicleScript.getUserLoccationInitial();
    }
    public float getHealth()
    {
        return UserVehicleScript.getHealthInitial();
    }

    public Collider getMeshCollider()
    {
        if (UserVehicle == null)
        {
            return null;
        }

        return UserVehicle.GetComponentInChildren<Collider>(); // Prometeo keeps some colliders on child objects.
    }

    //This function was CODED WITH THE ASSISTANCE OF AI
    //It essentially gets the user's collider's edges so that rays can be sent to the edge of the collider
    //If even one ray hits a single bound (this is done in CarDriveMechs), the user becomes found
    //This logic ensures that any part of the car counts as a hit. If the code only sent one ray to the center instead,
    //the cop car wouldn't see the user car if there was just a very thin wall that obstructed the center point.
    public Bounds getUserBounds()
    {
        Collider[] colliders = UserVehicle.GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            return new Bounds(UserVehicle.transform.position, Vector3.one);
        }

        Bounds bounds = colliders[0].bounds;

        for (int i = 1; i < colliders.Length; i++)
        {
            bounds.Encapsulate(colliders[i].bounds);
        }

        return bounds;
    }
    

//Camera Movement Getters
    public int getCameraMode()
    {
        return CameraScript.getCameraModeInitial();
    }

//CopCar Driver Script
    public float getHitTimer()
    {
        return CopCarScript.getHitTimerInitial();
    }

    public float getSpeedCop()
    {
        return CopCarScript.getSpeedInitial();
    }

    public Vector3 getLastKnownLocation()
    {
        return lastKnownLocation;
    }

    public bool getUserFound()
    {
        return isUserFound;
    }

//Spawner Script Getters
    public int getNumCars()
    {
        return CopCarSpawnerScript.getNumCarsInitial();
    }

    public List<GameObject> getAllCops()
    {
        return CopCarSpawnerScript.getInitialAllCops();
    }

//BOSS GETTERS
    public bool getCaught()
    {
        return caught;
    }

    //This function returns a list of gameobjects that exist on a certain layer (set in the object on Unity)
    //The primary user of this function is the Spawner. 
    //The spawner uses it so that the Spawner can generate a random coordinate on a specific LAYER
    public List<GameObject> findGameObjectsInLayer(int layer) 
    {
        //Find All gameobjects
        GameObject[] gameObjectList = FindObjectsOfType<GameObject>();
        //Make a list of gameobjects
        List<GameObject> gameObjectOnLayer = new List<GameObject>();
        //Iterate through all gameobjects and find the one with the specific layer. If it matches, add them to the list
        for (int i = 0; i < gameObjectList.Length; i++) {
            if (gameObjectList[i].layer == layer) {
                gameObjectOnLayer.Add(gameObjectList[i]);
            }
        }
    return gameObjectOnLayer;
    } 


    


    
    
}
