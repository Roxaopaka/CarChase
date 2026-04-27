using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameObjManager : MonoBehaviour
{

    public GameObject UserVehicle;
    public GameObject Camera;
    public GameObject CopCar;
    public GameObject CopCarSpawner;

    public UserVehicle UserVehicleScript;
    public cameraMovement CameraScript;
    public CarDriveMechs CopCarScript;
    public Spawner CopCarSpawnerScript;
    public bool caught=false;

    private bool isUserFound;

    private int numCarsSee;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserVehicleScript = UserVehicle.GetComponent<UserVehicle>();
        CameraScript = Camera.GetComponent<cameraMovement>();
        CopCarScript = CopCar.GetComponent<CarDriveMechs>();
        CopCarSpawnerScript = CopCarSpawner.GetComponent<Spawner>();
        numCarsSee = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (UserVehicle == null)
        {
            caught = true;
        }
        List<GameObject> allCops = getAllCops();

        for(int i = 0; i < allCops.Count; i++)
        {
            if (allCops[i].GetComponent<CarDriveMechs>().getUserFoundInitial()==true)
            {
                numCarsSee++;
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
        return UserVehicle.GetComponent<MeshCollider>();
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
        return CopCarScript.getLastKnownLocationInitial();
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


    
    
}
