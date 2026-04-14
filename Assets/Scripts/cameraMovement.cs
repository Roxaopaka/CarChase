using UnityEngine;

public class cameraMovement : MonoBehaviour //This is the script for the camera
{

    public GameObject userCar; //<-- Get the user's car object. 
    private Camera cam;

    private int cameraMode;  //This is the variable to find out which camera orientation to have

    private Vector3 mode1 = new Vector3(60,38,53.91f); //<-- This is the STARTING position for camera option 1 (Up and sideways tilted down)
    private Vector3 mode2 = new Vector3(35,3,54.91f); //<-- This is the STARTINg position for camera option 2 (First person)

    private Vector3 cameraOffsetModel1; //<-- We need this to store the DIFFERENCE in user - car position so that we can apply this offset to any other position
    private Vector3 cameraOffsetMode2; //<-- We need this to store the DIFFERENCE in user - car position so that we can apply this offset to any other position
    public GameObjManager boss;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();

        cameraOffsetModel1 = mode1 - userCar.transform.position; //<-- This is the offset (difference in position) for camera mode 1
        cameraOffsetMode2 = mode2 - userCar.transform.position; //<-- This is the offset for camera mode 2
        transform.position = mode1; //<-- The camera will first start in mode 1

    }
    //PRECONDITION: Camera object exists in the scene 
    void Update()
    {

        cameraMode = boss.getIteration(); //Get iteration returns 0 or 1. If it is 0, camera mode 1 is enabled. If it is 1, camera mode 2 is enabled.

        if (cameraMode == 0)
        {
            transform.position = userCar.transform.position+cameraOffsetModel1; //Apply the offset to any position every frame
            transform.rotation = Quaternion.Euler(50f, -90f, 0f); //The rotatioin for camera mode 1 is constant, so we don't change it
            cam.fieldOfView = 50f; //Field of view is 50

        }
        else if(cameraMode == 1)
        {
            transform.position = userCar.transform.position + userCar.transform.TransformDirection(cameraOffsetMode2); //Apply the offset. We use TransformDirection because we want WORLD COORDINATES, as the camera's rotation will change here.
            transform.rotation = userCar.transform.rotation; //<-- The rotation is the same as the user car's rotation
            cam.fieldOfView = 50f; //Field of view is 50
        }
        

    }
    //POSTCONDITION: Camera follows the user's car in either mode 1 or mode 2

    public int getCameraModeInitial()
    {
        return cameraMode;
    }

}
