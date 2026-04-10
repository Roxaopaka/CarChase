using UnityEngine;

public class UIManager : MonoBehaviour
{

    public GameObject copSpawner;

    private Spawner script;

    private int currentStar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        script = copSpawner.GetComponent<Spawner>();
        currentStar = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (script.getNumCars()-currentStar == 1)
        {
            addStars();
        }
    }

    private void addStars()
    {
        
    }
}
