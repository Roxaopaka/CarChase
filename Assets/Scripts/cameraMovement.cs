using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public GameObject userCar;
    private Vector3 userPosition;
    private Vector3 cameraOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraOffset = transform.position - userCar.transform.position;
    }

    void Update()
    {
        transform.position = userCar.transform.position+cameraOffset;

    }
}
