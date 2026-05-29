/*
MESSAGE FROM CREATOR: This script was coded by Mena. You can use it in your games either these are commercial or
personal projects. You can even add or remove functions as you wish. However, you cannot sell copies of this
script by itself, since it is originally distributed as a free product.
I wish you the best for your project. Good luck!

P.S: If you need more cars, you can check my other vehicle assets on the Unity Asset Store, perhaps you could find
something useful for your game. Best regards, Mena.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrometeoCarController : MonoBehaviour
{
    public GameObjManager boss;

    public int wantedLevel;
    public float startingHealth=200f;
    public float minimumCrashSpeedForDamage = 8f;
    public float crashDamageMultiplier = 0.9f;
    private float count;
    private int iteration;
    private float health;


    //CAR SETUP

      [Space(20)]
      //[Header("CAR SETUP")]
      [Space(10)]
      [Range(20, 5000)]
      public int maxSpeed = 140; //The maximum speed that the car can reach in km/h.
      [Range(10, 120)]
      public int maxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.
      [Range(0.1f, 1000f)]
      public float accelerationMultiplier = 2f; // How fast the car can accelerate. 0.5 is slow and higher values accelerate faster.
      [Space(10)]
      [Range(10, 45)]
      public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
      [Range(0.1f, 1f)]
      public float steeringSpeed = 0.5f; // How fast the steering wheel turns.
      [Space(10)]
      [Range(100, 5000)]
      public int brakeForce = 5000; // The strength of the wheel brakes.
      [Range(1f, 80f)]
      public float brakeDecelerationForce = 35f; // Extra Rigidbody braking force used when switching between forward and reverse.
      [Range(1f, 5f)]
      public float steeringResponseMultiplier = 2f; // Makes the car steer less stiffly.
      [Range(0f, 10f)]
      public float steeringAssist = 3f; // Helps the car body rotate in the steering direction.
      [Range(1, 10)]
      public int decelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.
      [Range(0f, 30f)]
      public float sidewaysGripRecovery = 8f; // How strongly the car corrects sideways sliding when handbrake is not held.
      [Range(1, 10)]
      public int handbrakeDriftMultiplier = 5; // How much grip the car loses when the user hit the handbrake.
      [Range(0.5f, 8f)]
      public float driftBuildSpeed = 2.5f; // How quickly the car enters a drift.
      [Range(0.5f, 100f)]
      public float driftRecoverySpeed = 1.8f; // How quickly the car regains grip after drifting.
      [Range(0f, 1f)]
      public float frontDriftGripLoss = 0.25f; // Front wheels keep more grip so the car can hold a drift.
      [Range(0f, 1f)]
      public float rearDriftGripLoss = 0.65f; // Rear wheels lose more grip so the car slides controllably.
      [Range(0f, 8f)]
      public float driftYawAssist = 1.5f; // Extra controllable rotation while drifting.
      [Range(0f, 20f)]
      public float driftForwardAssist = 4f; // Helps maintain speed through longer drifts.
      [Space(10)]
      public Vector3 bodyMassCenter; // This is a vector that contains the center of mass of the car. I recommend to set this value
                                    // in the points x = 0 and z = 0 of your car. You can select the value that you want in the y axis,
                                    // however, you must notice that the higher this value is, the more unstable the car becomes.
                                    // Usually the y value goes from 0 to 1.5.

    //WHEELS

      //[Header("WHEELS")]

      /*
      The following variables are used to store the wheels' data of the car. We need both the mesh-only game objects and wheel
      collider components of the wheels. The wheel collider components and 3D meshes of the wheels cannot come from the same
      game object; they must be separate game objects.
      */
      public GameObject frontLeftMesh;
      public WheelCollider frontLeftCollider;
      [Space(10)]
      public GameObject frontRightMesh;
      public WheelCollider frontRightCollider;
      [Space(10)]
      public GameObject rearLeftMesh;
      public WheelCollider rearLeftCollider;
      [Space(10)]
      public GameObject rearRightMesh;
      public WheelCollider rearRightCollider;

    //PARTICLE SYSTEMS

      [Space(20)]
      //[Header("EFFECTS")]
      [Space(10)]
      //The following variable lets you to set up particle systems in your car
      public bool useEffects = false;

      // The following particle systems are used as tire smoke when the car drifts.
      public ParticleSystem RLWParticleSystem;
      public ParticleSystem RRWParticleSystem;

      [Space(10)]
      // The following trail renderers are used as tire skids when the car loses traction.
      public TrailRenderer RLWTireSkid;
      public TrailRenderer RRWTireSkid;

    //SPEED TEXT (UI)

      [Space(20)]
      //[Header("UI")]
      [Space(10)]
      //The following variable lets you to set up a UI text to display the speed of your car.
      public bool useUI = false;
      public Text carSpeedText; // Used to store the UI object that is going to show the speed of the car.

    //SOUNDS

      [Space(20)]
      //[Header("Sounds")]
      [Space(10)]
      //The following variable lets you to set up sounds for your car such as the car engine or tire screech sounds.
      public bool useSounds = false;
      public AudioSource carEngineSound; // This variable stores the sound of the car engine.
      public AudioSource tireScreechSound; // This variable stores the sound of the tire screech (when the car is drifting).
      float initialCarEngineSoundPitch; // Used to store the initial pitch of the car engine sound.

    //CONTROLS

      [Space(20)]
      //[Header("CONTROLS")]
      [Space(10)]
      //The following variables lets you to set up touch controls for mobile devices.
      public bool useTouchControls = false;
      public GameObject throttleButton;
      PrometeoTouchInput throttlePTI;
      public GameObject reverseButton;
      PrometeoTouchInput reversePTI;
      public GameObject turnRightButton;
      PrometeoTouchInput turnRightPTI;
      public GameObject turnLeftButton;
      PrometeoTouchInput turnLeftPTI;
      public GameObject handbrakeButton;
      PrometeoTouchInput handbrakePTI;

    //CAR DATA

      [HideInInspector]
      public float carSpeed; // Used to store the speed of the car.
      [HideInInspector]
      public bool isDrifting; // Used to know whether the car is drifting or not.
      [HideInInspector]
      public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.

    //PRIVATE VARIABLES

      /*
      IMPORTANT: The following variables should not be modified manually since their values are automatically given via script.
      */
      Rigidbody carRigidbody; // Stores the car's rigidbody.
      float steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
      float throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
      float driftingAxis;
      float localVelocityZ;
      float localVelocityX;
      bool deceleratingCar;
      bool touchControlsSetup = false;
      bool throttleInput;
      bool reverseInput;
      bool turnLeftInput;
      bool turnRightInput;
      bool handbrakeInput;
      bool handbrakeReleased;
      bool previousHandbrakeInput;
      /*
      The following variables are used to store information about sideways friction of the wheels (such as
      extremumSlip,extremumValue, asymptoteSlip, asymptoteValue and stiffness). We change this values to
      make the car to start drifting.
      */
      WheelFrictionCurve FLwheelFriction;
      float FLWextremumSlip;
      float FLWstiffness;
      WheelFrictionCurve FRwheelFriction;
      float FRWextremumSlip;
      float FRWstiffness;
      WheelFrictionCurve RLwheelFriction;
      float RLWextremumSlip;
      float RLWstiffness;
      WheelFrictionCurve RRwheelFriction;
      float RRWextremumSlip;
      float RRWstiffness;

    // Start is called before the first frame update
    void Start()
    {
      //In this part, we set the 'carRigidbody' value with the Rigidbody attached to this
      //gameObject. Also, we define the center of mass of the car with the Vector3 given
      //in the inspector.
      carRigidbody = gameObject.GetComponent<Rigidbody>();
      carRigidbody.centerOfMass = bodyMassCenter;
      health = startingHealth;

      //Initial setup to calculate the drift value of the car. This part could look a bit
      //complicated, but do not be afraid, the only thing we're doing here is to save the default
      //friction values of the car wheels so we can set an appropiate drifting value later.
      FLwheelFriction = new WheelFrictionCurve ();
        FLwheelFriction.extremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLWextremumSlip = frontLeftCollider.sidewaysFriction.extremumSlip;
        FLwheelFriction.extremumValue = frontLeftCollider.sidewaysFriction.extremumValue;
        FLwheelFriction.asymptoteSlip = frontLeftCollider.sidewaysFriction.asymptoteSlip;
        FLwheelFriction.asymptoteValue = frontLeftCollider.sidewaysFriction.asymptoteValue;
        FLwheelFriction.stiffness = frontLeftCollider.sidewaysFriction.stiffness;
        FLWstiffness = frontLeftCollider.sidewaysFriction.stiffness;
      FRwheelFriction = new WheelFrictionCurve ();
        FRwheelFriction.extremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRWextremumSlip = frontRightCollider.sidewaysFriction.extremumSlip;
        FRwheelFriction.extremumValue = frontRightCollider.sidewaysFriction.extremumValue;
        FRwheelFriction.asymptoteSlip = frontRightCollider.sidewaysFriction.asymptoteSlip;
        FRwheelFriction.asymptoteValue = frontRightCollider.sidewaysFriction.asymptoteValue;
        FRwheelFriction.stiffness = frontRightCollider.sidewaysFriction.stiffness;
        FRWstiffness = frontRightCollider.sidewaysFriction.stiffness;
      RLwheelFriction = new WheelFrictionCurve ();
        RLwheelFriction.extremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLWextremumSlip = rearLeftCollider.sidewaysFriction.extremumSlip;
        RLwheelFriction.extremumValue = rearLeftCollider.sidewaysFriction.extremumValue;
        RLwheelFriction.asymptoteSlip = rearLeftCollider.sidewaysFriction.asymptoteSlip;
        RLwheelFriction.asymptoteValue = rearLeftCollider.sidewaysFriction.asymptoteValue;
        RLwheelFriction.stiffness = rearLeftCollider.sidewaysFriction.stiffness;
        RLWstiffness = rearLeftCollider.sidewaysFriction.stiffness;
      RRwheelFriction = new WheelFrictionCurve ();
        RRwheelFriction.extremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRWextremumSlip = rearRightCollider.sidewaysFriction.extremumSlip;
        RRwheelFriction.extremumValue = rearRightCollider.sidewaysFriction.extremumValue;
        RRwheelFriction.asymptoteSlip = rearRightCollider.sidewaysFriction.asymptoteSlip;
        RRwheelFriction.asymptoteValue = rearRightCollider.sidewaysFriction.asymptoteValue;
        RRwheelFriction.stiffness = rearRightCollider.sidewaysFriction.stiffness;
        RRWstiffness = rearRightCollider.sidewaysFriction.stiffness;

        // We save the initial pitch of the car engine sound.
        if(carEngineSound != null){
          initialCarEngineSoundPitch = carEngineSound.pitch;
          carEngineSound.loop = true;
        }

        // We invoke 2 methods inside this script. CarSpeedUI() changes the text of the UI object that stores
        // the speed of the car and CarSounds() controls the engine and drifting sounds. Both methods are invoked
        // in 0 seconds, and repeatedly called every 0.1 seconds.
        if(useUI){
          InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }else if(!useUI){
          if(carSpeedText != null){
            carSpeedText.text = "0";
          }
        }

        if(useSounds){
          InvokeRepeating("CarSounds", 0f, 0.1f);
        }else if(!useSounds){
          if(carEngineSound != null){
            carEngineSound.Stop();
          }
          if(tireScreechSound != null){
            tireScreechSound.Stop();
          }
        }

        if(!useEffects){
          if(RLWParticleSystem != null){
            RLWParticleSystem.Stop();
          }
          if(RRWParticleSystem != null){
            RRWParticleSystem.Stop();
          }
          if(RLWTireSkid != null){
            RLWTireSkid.emitting = false;
          }
          if(RRWTireSkid != null){
            RRWTireSkid.emitting = false;
          }
        }

        if(useTouchControls){
          if(throttleButton != null && reverseButton != null &&
          turnRightButton != null && turnLeftButton != null
          && handbrakeButton != null){

            throttlePTI = throttleButton.GetComponent<PrometeoTouchInput>();
            reversePTI = reverseButton.GetComponent<PrometeoTouchInput>();
            turnLeftPTI = turnLeftButton.GetComponent<PrometeoTouchInput>();
            turnRightPTI = turnRightButton.GetComponent<PrometeoTouchInput>();
            handbrakePTI = handbrakeButton.GetComponent<PrometeoTouchInput>();
            touchControlsSetup = true;

          }else{
           
          }
        }

    }

    // Update is called once per rendered frame. Input is read here so key presses are not missed.
    void Update()
    {

      UpdateCarData();
      ReadDrivingInput();

      //RION EDITED
      if (carSpeed > 80 && wantedLevel==0)
        {
            wantedLevel=1;
        }

        if (wantedLevel > 0)
        {
            count+=Time.deltaTime;
            if (count > 10)
            {
                wantedLevel++;
                count-=10;
            }
        }

        if (boss.getCaught() == true)
        {
            Destroy(gameObject);
        }

      // We call the method AnimateWheelMeshes() in order to match the wheel collider movements with the 3D meshes of the wheels.
      AnimateWheelMeshes();

    }

    // FixedUpdate is called at Unity's physics rate. Rigidbody and WheelCollider changes belong here.
    void FixedUpdate()
    {
      UpdateCarData();
      ApplyDrivingInput();
    }

    void UpdateCarData()
    {
      // We determine the speed of the car.
      carSpeed = carRigidbody.linearVelocity.magnitude * 3.6f;
      // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
      localVelocityX = transform.InverseTransformDirection(carRigidbody.linearVelocity).x;
      // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
      localVelocityZ = transform.InverseTransformDirection(carRigidbody.linearVelocity).z;
    }

    void ReadDrivingInput()
    {
      if (useTouchControls && touchControlsSetup){
        throttleInput = throttlePTI.buttonPressed;
        reverseInput = reversePTI.buttonPressed;
        turnLeftInput = turnLeftPTI.buttonPressed;
        turnRightInput = turnRightPTI.buttonPressed;
        handbrakeInput = handbrakePTI.buttonPressed;
      }else{
        throttleInput = Input.GetKey(KeyCode.W);
        reverseInput = Input.GetKey(KeyCode.S);
        turnLeftInput = Input.GetKey(KeyCode.A);
        turnRightInput = Input.GetKey(KeyCode.D);
        handbrakeInput = Input.GetKey(KeyCode.Space);
      }

      handbrakeReleased = previousHandbrakeInput && !handbrakeInput;
      previousHandbrakeInput = handbrakeInput;
    }

    void ApplyDrivingInput()
    {
        if(throttleInput){
          CancelInvoke("DecelerateCar");
          deceleratingCar = false;
          GoForward();
        }
        if(reverseInput){
          CancelInvoke("DecelerateCar");
          deceleratingCar = false;
          GoReverse();
        }

        if(turnLeftInput){
          TurnLeft();
        }
        if(turnRightInput){
          TurnRight();
        }
        if(handbrakeInput){
          CancelInvoke("DecelerateCar");
          deceleratingCar = false;
          Handbrake();
        }
        if(handbrakeReleased){
          RecoverTraction();
        }
        if((!throttleInput && !reverseInput)){
          ThrottleOff();
        }
        if((!reverseInput && !throttleInput) && !handbrakeInput && !deceleratingCar){
          InvokeRepeating("DecelerateCar", 0f, 0.1f);
          deceleratingCar = true;
        }
        if(!turnLeftInput && !turnRightInput && steeringAxis != 0f){
          ResetSteeringAngle();
        }
        ApplySidewaysGripRecovery();
    }

    void ApplySidewaysGripRecovery()
    {
      if(handbrakeInput || sidewaysGripRecovery <= 0f){
        return;
      }

      Vector3 sidewaysVelocity = transform.right * localVelocityX;
      carRigidbody.AddForce(-sidewaysVelocity * sidewaysGripRecovery, ForceMode.Acceleration);
    }

    // This method converts the car speed data from float to string, and then set the text of the UI carSpeedText with this value.
    public void CarSpeedUI(){

      if(useUI && carSpeedText != null){
        float absoluteCarSpeed = Mathf.Abs(carSpeed);
        carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
      }

    }

    // This method controls the car sounds. For example, the car engine will sound slow when the car speed is low because the
    // pitch of the sound will be at its lowest point. On the other hand, it will sound fast when the car speed is high because
    // the pitch of the sound will be the sum of the initial pitch + the car speed divided by 100f.
    // Apart from that, the tireScreechSound will play whenever the car starts drifting or losing traction.
    public void CarSounds(){

      if(useSounds){
        if(carEngineSound != null){
          float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.linearVelocity.magnitude) / 25f);
          carEngineSound.pitch = engineSoundPitch;
        }
        if(tireScreechSound != null){
          if((isDrifting) || (isTractionLocked && Mathf.Abs(carSpeed) > 12f)){
            if(!tireScreechSound.isPlaying){
              tireScreechSound.Play();
            }
          }else if((!isDrifting) && (!isTractionLocked || Mathf.Abs(carSpeed) < 12f)){
            tireScreechSound.Stop();
          }
        }
      }else if(!useSounds){
        if(carEngineSound != null && carEngineSound.isPlaying){
          carEngineSound.Stop();
        }
        if(tireScreechSound != null && tireScreechSound.isPlaying){
          tireScreechSound.Stop();
        }
      }

    }

    void PlayEngineSound(){
      if(useSounds && carEngineSound != null && !carEngineSound.isPlaying){
        carEngineSound.Play();
      }
    }

    void StopEngineSound(){
      if(carEngineSound != null && carEngineSound.isPlaying){
        carEngineSound.Stop();
      }
    }

    //
    //STEERING METHODS
    //

    //The following method turns the front car wheels to the left. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnLeft(){
      steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed * steeringResponseMultiplier);
      if(steeringAxis < -1f){
        steeringAxis = -1f;
      }
      var steeringAngle = steeringAxis * maxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed * steeringResponseMultiplier);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed * steeringResponseMultiplier);
      ApplySteeringAssist();
    }

    //The following method turns the front car wheels to the right. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnRight(){
      steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed * steeringResponseMultiplier);
      if(steeringAxis > 1f){
        steeringAxis = 1f;
      }
      var steeringAngle = steeringAxis * maxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed * steeringResponseMultiplier);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed * steeringResponseMultiplier);
      ApplySteeringAssist();
    }

    //The following method takes the front car wheels to their default position (rotation = 0). The speed of this movement will depend
    // on the steeringSpeed variable.
    public void ResetSteeringAngle(){
      if(steeringAxis < 0f){
        steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
      }else if(steeringAxis > 0f){
        steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
      }
      if(Mathf.Abs(frontLeftCollider.steerAngle) < 1f){
        steeringAxis = 0f;
      }
      var steeringAngle = steeringAxis * maxSteeringAngle;
      frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
      frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
    }

    void ApplySteeringAssist(){
      float speedFactor = Mathf.Clamp01(carRigidbody.linearVelocity.magnitude / 25f);
      float driftAssistLimit = Mathf.Lerp(1f, 0.35f, driftingAxis);
      carRigidbody.AddTorque(Vector3.up * steeringAxis * steeringAssist * driftAssistLimit * speedFactor, ForceMode.Acceleration);
    }

    // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
    void AnimateWheelMeshes(){
      if(frontLeftCollider != null && frontLeftMesh != null){
        Quaternion FLWRotation;
        Vector3 FLWPosition;
        frontLeftCollider.GetWorldPose(out FLWPosition, out FLWRotation);
        frontLeftMesh.transform.position = FLWPosition;
        frontLeftMesh.transform.rotation = FLWRotation;
      }

      if(frontRightCollider != null && frontRightMesh != null){
        Quaternion FRWRotation;
        Vector3 FRWPosition;
        frontRightCollider.GetWorldPose(out FRWPosition, out FRWRotation);
        frontRightMesh.transform.position = FRWPosition;
        frontRightMesh.transform.rotation = FRWRotation;
      }

      if(rearLeftCollider != null && rearLeftMesh != null){
        Quaternion RLWRotation;
        Vector3 RLWPosition;
        rearLeftCollider.GetWorldPose(out RLWPosition, out RLWRotation);
        rearLeftMesh.transform.position = RLWPosition;
        rearLeftMesh.transform.rotation = RLWRotation;
      }

      if(rearRightCollider != null && rearRightMesh != null){
        Quaternion RRWRotation;
        Vector3 RRWPosition;
        rearRightCollider.GetWorldPose(out RRWPosition, out RRWRotation);
        rearRightMesh.transform.position = RRWPosition;
        rearRightMesh.transform.rotation = RRWRotation;
      }
    }

    //
    //ENGINE AND BRAKING METHODS
    //

    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward(){
      PlayEngineSound();
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car is losing traction, then the car will start emitting particle systems.
      if(Mathf.Abs(localVelocityX) > 40){
        isDrifting = true;
        DriftCarPS();
      }else{
        isDrifting = false;
        DriftCarPS();
      }
      // The following part sets the throttle power to 1 smoothly.
      throttleAxis = throttleAxis + (Time.deltaTime * 3f);
      if(throttleAxis > 1f){
        throttleAxis = 1f;
      }
      //If the car is going backwards, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is less than -1f, then it
      //is safe to apply positive torque to go forward.
      if(localVelocityZ < -1f){
        Brakes();
      }else{
        if(Mathf.RoundToInt(carSpeed) < maxSpeed){
        //Apply positive torque in all wheels to go forward if maxSpeed has not been reached.
        frontLeftCollider.brakeTorque = 0;
        frontRightCollider.brakeTorque = 0;
        rearLeftCollider.brakeTorque = 0;
        rearRightCollider.brakeTorque = 0;

        float torque = (accelerationMultiplier * 50f) * throttleAxis;

        frontLeftCollider.motorTorque = torque;
        frontRightCollider.motorTorque = torque;
        rearLeftCollider.motorTorque = torque;
        rearRightCollider.motorTorque = torque;

        Vector3 forwardForce = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        carRigidbody.AddForce(forwardForce * accelerationMultiplier * 10f * throttleAxis, ForceMode.Acceleration);        
        }else {
          // If the maxSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
    			frontLeftCollider.motorTorque = 0;
    			frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
    			rearRightCollider.motorTorque = 0;
    		}
      }
    }

    // This method apply negative torque to the wheels in order to go backwards.
    public void GoReverse(){
      PlayEngineSound();
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car is losing traction, then the car will start emitting particle systems.
      if(Mathf.Abs(localVelocityX) > 30f){
        isDrifting = true;
        DriftCarPS();
      }else{
        isDrifting = false;
        DriftCarPS();
      }
      // The following part sets the throttle power to -1 smoothly.
      throttleAxis = throttleAxis - (Time.deltaTime * 3f);
      if(throttleAxis < -1f){
        throttleAxis = -1f;
      }
      //If the car is still going forward, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is greater than 1f, then it
      //is safe to apply negative torque to go reverse.
      if(localVelocityZ > 1f){
        Brakes();
      }else{
        if(Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed){
        frontLeftCollider.brakeTorque = 0;
        frontRightCollider.brakeTorque = 0;
        rearLeftCollider.brakeTorque = 0;
        rearRightCollider.brakeTorque = 0;

        float torque = (accelerationMultiplier * 50f) * throttleAxis;

        frontLeftCollider.motorTorque = torque;
        frontRightCollider.motorTorque = torque;
        rearLeftCollider.motorTorque = torque;
        rearRightCollider.motorTorque = torque;

        Vector3 reverseForce = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        carRigidbody.AddForce(reverseForce * accelerationMultiplier * 10f * throttleAxis, ForceMode.Acceleration);
        }else {
          //If the maxReverseSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxReverseSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
    			frontLeftCollider.motorTorque = 0;
    			frontRightCollider.motorTorque = 0;
          rearLeftCollider.motorTorque = 0;
    			rearRightCollider.motorTorque = 0;
    		}
      }
    }

    //The following function set the motor torque to 0 (in case the user is not pressing either W or S).
    public void ThrottleOff(){
      StopEngineSound();
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;
    }

    // The following method decelerates the speed of the car according to the decelerationMultiplier variable, where
    // 1 is the slowest and 10 is the fastest deceleration. This method is called by the function InvokeRepeating,
    // usually every 0.1f when the user is not pressing W (throttle), S (reverse) or Space bar (handbrake).
    public void DecelerateCar(){
      if(Mathf.Abs(localVelocityX) > 2.5f){
        isDrifting = true;
        DriftCarPS();
      }else{
        isDrifting = false;
        DriftCarPS();
      }
      // The following part resets the throttle power to 0 smoothly.
      if(throttleAxis != 0f){
        if(throttleAxis > 0f){
          throttleAxis = throttleAxis - (Time.deltaTime * 10f);
        }else if(throttleAxis < 0f){
            throttleAxis = throttleAxis + (Time.deltaTime * 10f);
        }
        if(Mathf.Abs(throttleAxis) < 0.15f){
          throttleAxis = 0f;
        }
      }
      Vector3 horizontalVelocity = Vector3.ProjectOnPlane(carRigidbody.linearVelocity, Vector3.up);

      if(horizontalVelocity.magnitude > 0.1f){
        carRigidbody.AddForce(-horizontalVelocity.normalized * decelerationMultiplier * 5f, ForceMode.Acceleration);
      }
      // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;
      // If the magnitude of the car's velocity is less than 0.25f (very slow velocity), then stop the car completely and
      // also cancel the invoke of this method.
      if(carRigidbody.linearVelocity.magnitude < 0.25f){
        carRigidbody.linearVelocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }

    // This function applies brake torque to the wheels according to the brake force given by the user.
    public void Brakes(){
      frontLeftCollider.motorTorque = 0;
      frontRightCollider.motorTorque = 0;
      rearLeftCollider.motorTorque = 0;
      rearRightCollider.motorTorque = 0;

      frontLeftCollider.brakeTorque = brakeForce;
      frontRightCollider.brakeTorque = brakeForce;
      rearLeftCollider.brakeTorque = brakeForce;
      rearRightCollider.brakeTorque = brakeForce;

      Vector3 horizontalVelocity = Vector3.ProjectOnPlane(carRigidbody.linearVelocity, Vector3.up);
      if(horizontalVelocity.magnitude > 0.1f){
        float brakingAcceleration = brakeDecelerationForce * Mathf.Max(1f, brakeForce / 1500f);
        carRigidbody.AddForce(-horizontalVelocity.normalized * brakingAcceleration, ForceMode.Acceleration);
      }
    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    public void Handbrake(){
      CancelInvoke("RecoverTraction");
      driftingAxis = Mathf.MoveTowards(driftingAxis, 1f, Time.deltaTime * driftBuildSpeed);
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car lost its traction, then the car will start emitting particle systems.
      if(Mathf.Abs(localVelocityX) > 2.5f){
        isDrifting = true;
      }else{
        isDrifting = false;
      }
      ApplyDriftFriction();
      ApplyDriftAssist();

      // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
      // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
      isTractionLocked = true;
      DriftCarPS();

    }

    void ApplyDriftFriction(){
      float frontSlipMultiplier = Mathf.Lerp(1f, 1f + (handbrakeDriftMultiplier * frontDriftGripLoss), driftingAxis);
      float rearSlipMultiplier = Mathf.Lerp(1f, 1f + (handbrakeDriftMultiplier * rearDriftGripLoss), driftingAxis);
      float frontStiffnessMultiplier = Mathf.Lerp(1f, 1f - frontDriftGripLoss, driftingAxis);
      float rearStiffnessMultiplier = Mathf.Lerp(1f, 1f - rearDriftGripLoss, driftingAxis);

      FLwheelFriction.extremumSlip = FLWextremumSlip * frontSlipMultiplier;
      FLwheelFriction.stiffness = FLWstiffness * Mathf.Max(0.2f, frontStiffnessMultiplier);
      frontLeftCollider.sidewaysFriction = FLwheelFriction;

      FRwheelFriction.extremumSlip = FRWextremumSlip * frontSlipMultiplier;
      FRwheelFriction.stiffness = FRWstiffness * Mathf.Max(0.2f, frontStiffnessMultiplier);
      frontRightCollider.sidewaysFriction = FRwheelFriction;

      RLwheelFriction.extremumSlip = RLWextremumSlip * rearSlipMultiplier;
      RLwheelFriction.stiffness = RLWstiffness * Mathf.Max(0.2f, rearStiffnessMultiplier);
      rearLeftCollider.sidewaysFriction = RLwheelFriction;

      RRwheelFriction.extremumSlip = RRWextremumSlip * rearSlipMultiplier;
      RRwheelFriction.stiffness = RRWstiffness * Mathf.Max(0.2f, rearStiffnessMultiplier);
      rearRightCollider.sidewaysFriction = RRwheelFriction;
    }

    void ApplyDriftAssist(){
      Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
      float speedFactor = Mathf.Clamp01(carRigidbody.linearVelocity.magnitude / 20f);

      if(flatForward.sqrMagnitude > 0.01f){
        carRigidbody.AddForce(flatForward * driftForwardAssist * driftingAxis * speedFactor, ForceMode.Acceleration);
      }

      carRigidbody.AddTorque(Vector3.up * steeringAxis * driftYawAssist * driftingAxis * speedFactor, ForceMode.Acceleration);
      carRigidbody.angularVelocity = new Vector3(
        carRigidbody.angularVelocity.x,
        Mathf.Clamp(carRigidbody.angularVelocity.y, -2.5f, 2.5f),
        carRigidbody.angularVelocity.z
      );
    }

    // This function is used to emit both the particle systems of the tires' smoke and the trail renderers of the tire skids
    // depending on the value of the bool variables 'isDrifting' and 'isTractionLocked'.
    public void DriftCarPS(){

      if(useEffects){
        if(RLWParticleSystem != null && RRWParticleSystem != null){
          if(isDrifting){
            RLWParticleSystem.Play();
            RRWParticleSystem.Play();
          }else if(!isDrifting){
            RLWParticleSystem.Stop();
            RRWParticleSystem.Stop();
          }
        }

        if(RLWTireSkid != null && RRWTireSkid != null){
          if((isTractionLocked || Mathf.Abs(localVelocityX) > 5f) && Mathf.Abs(carSpeed) > 12f){
            RLWTireSkid.emitting = true;
            RRWTireSkid.emitting = true;
          }else {
            RLWTireSkid.emitting = false;
            RRWTireSkid.emitting = false;
          }
        }
      }else if(!useEffects){
        if(RLWParticleSystem != null){
          RLWParticleSystem.Stop();
        }
        if(RRWParticleSystem != null){
          RRWParticleSystem.Stop();
        }
        if(RLWTireSkid != null){
          RLWTireSkid.emitting = false;
        }
        if(RRWTireSkid != null){
          RRWTireSkid.emitting = false;
        }
      }

    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    public void RecoverTraction(){
      isTractionLocked = false;
      driftingAxis = Mathf.MoveTowards(driftingAxis, 0f, Time.deltaTime * driftRecoverySpeed);

      //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
      //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
      // car's grip.
      if(driftingAxis > 0f){
        ApplyDriftFriction();
        Invoke("RecoverTraction", Time.deltaTime);
      }else {
        FLwheelFriction.extremumSlip = FLWextremumSlip;
        FLwheelFriction.stiffness = FLWstiffness;
        frontLeftCollider.sidewaysFriction = FLwheelFriction;

        FRwheelFriction.extremumSlip = FRWextremumSlip;
        FRwheelFriction.stiffness = FRWstiffness;
        frontRightCollider.sidewaysFriction = FRwheelFriction;

        RLwheelFriction.extremumSlip = RLWextremumSlip;
        RLwheelFriction.stiffness = RLWstiffness;
        rearLeftCollider.sidewaysFriction = RLwheelFriction;

        RRwheelFriction.extremumSlip = RRWextremumSlip;
        RRwheelFriction.stiffness = RRWstiffness;
        rearRightCollider.sidewaysFriction = RRwheelFriction;

        driftingAxis = 0f;
      }
    }

    //RION EDITED
    public void OnTestKey()
    {
        if (iteration == 0)
        {
            iteration=1;
        }
        else
        {
            iteration=0;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CarDriveMechs copScript = collision.gameObject.GetComponentInParent<CarDriveMechs>();
        if (copScript != null)
        {
            float crashSpeed = Mathf.Abs((carSpeed * 0.5f) - copScript.getSpeedInitial());
            if (crashSpeed < minimumCrashSpeedForDamage)
            {
                return;
            }

            health -= crashSpeed * crashDamageMultiplier;
                        if (health < 0)
            {
                Destroy(gameObject);
                
            }
        }
    }

    public int getIterationInitial()
    {
        return iteration;
    }

    public int getWantedLevelInitial()
    {
        return wantedLevel;
    }

    public float getSpeedInitial()
    {
        return carSpeed;
    }

    public Vector3 getUserLoccationInitial()
    {
        return transform.position;
    }

    public float getHealthInitial()
    {
        return health;
    }

    

}
