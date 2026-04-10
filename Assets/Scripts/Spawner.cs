using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject copPreFab;
    private UserVehicle script;
    public GameObject userCar;

    private Vector3 spawnPosition; 
    private Quaternion rotation;

    private int currentWantedLevel;

    private int numCars;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = userCar.GetComponent<UserVehicle>();
        rotation = Quaternion.Euler(0f, 0f, 0f);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(script.getWantedLevel());
        if (currentWantedLevel - script.getWantedLevel() <0)
        {
            GameObject cops = Instantiate(copPreFab,this.transform);
            numCars++;
        }
        currentWantedLevel = script.getWantedLevel();
}
    public int getNumCars()
    {
        return numCars;
    }
}