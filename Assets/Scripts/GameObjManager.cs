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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserVehicleScript = UserVehicle.GetComponent<UserVehicle>();
        CameraScript = Camera.GetComponent<cameraMovement>();
        CopCarScript = CopCar.GetComponent<CarDriveMechs>();
        CopCarSpawnerScript = CopCarSpawner.GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public bool getHit()
    {
        return CopCarScript.getHitInitial();
    }
    public float getSpeedCop()
    {
        return CopCarScript.getSpeedInitial();
    }

//Spawner Script Getters
    public int getNumCars()
    {
        return CopCarSpawnerScript.getNumCarsInitial();
    }

    
    
}
