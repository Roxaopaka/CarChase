using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    private int currentStar;

    public GameObject panel;
    public GameObject wastedImage;

    public Image stars;

    public GameObjManager boss;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStar = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (boss.getNumCars()-currentStar == 1)
        {
            addStars();
            currentStar++;
        }
        if (boss.getHit() == true || boss.getHealth()<0)
        {
            panel.SetActive(true);
            wastedImage.SetActive(true);
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
