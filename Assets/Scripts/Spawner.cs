using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour //This is the script for the spawner of cop car objects
{

    public GameObject copPreFab; //Reference for the cop car prefab

    private int currentWantedLevel; //Store the current wanted level

    private int numCars; //Store the number of police cars
    public GameObjManager boss;

    public List<GameObject> allCops;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

}