using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    public GameObject copSpawner;

    private Spawner script;

    private int currentStar;

    public Image stars;

    private Vector3 location;
    private Quaternion rotation;
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
            currentStar++;
        }
    }

    private void addStars()
    {
        {
    GameObject star = Instantiate(stars,this.transform).gameObject;
    RectTransform rectTrans = star.GetComponent<RectTransform>();
    
    rectTrans.anchorMin = new Vector2(1,1);
    rectTrans.anchorMax = new Vector2(1,1);
    rectTrans.pivot = new Vector2(1,1);


    rectTrans.anchoredPosition = new Vector2(-50-(38.25f*currentStar),-5);
}
    }
}
