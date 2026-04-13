using UnityEngine;

public class Spawner : MonoBehaviour //This is the script for the spawner of cop car objects
{

    public GameObject copPreFab; //Reference for the cop car prefab
    private UserVehicle script; //We need the user vehicles script to know the current wanted level 
    public GameObject userCar; //Reference for the user's car

    private int currentWantedLevel; //Store the current wanted level

    private int numCars; //Store the number of police cars


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = userCar.GetComponent<UserVehicle>();
    }

    //Fixed update does not run every frame. Instead, it runs every 0.02 seconds
    void FixedUpdate()
    {
        Debug.Log(script.getWantedLevel());
        
        //This if statement is to check if the user's wanted level went up
        if (currentWantedLevel - script.getWantedLevel() <0)
        {
            //Instantiate the prefab
            Instantiate(copPreFab,this.transform);
            numCars++;
        }
        currentWantedLevel = script.getWantedLevel();
}
    //Getter method to return the number of police cars on the map
    public int getNumCars()
    {
        return numCars;
    }
}