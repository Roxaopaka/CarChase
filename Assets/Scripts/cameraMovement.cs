using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public GameObject userCar;
    private Camera cam;

    private int cameraMode;  
    private UserVehicle script;

    private Vector3 mode1 = new Vector3(60,38,53.91f);
    private Vector3 mode2 = new Vector3(35,3,54.91f);

    private Vector3 cameraOffsetModel1;
    private Vector3 cameraOffsetMode2;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
        script = userCar.GetComponent<UserVehicle>();
        cameraOffsetModel1 = mode1 - userCar.transform.position;
        cameraOffsetMode2 = mode2 - userCar.transform.position;
        transform.position = mode1;

    }

    void Update()
    {

        cameraMode = script.getIteration();
        if (cameraMode > 1)
        {
            cameraMode = 0;
        }

        if (cameraMode == 0)
        {
            transform.position = userCar.transform.position+cameraOffsetModel1;
            transform.rotation = Quaternion.Euler(50f, -90f, 0f);
            cam.fieldOfView = 50f;

        }
        else if(cameraMode == 1)
        {
            transform.position = userCar.transform.position+cameraOffsetMode2;
            transform.rotation = userCar.transform.rotation;
            cam.fieldOfView = 60f;
        }
        

    }
}
