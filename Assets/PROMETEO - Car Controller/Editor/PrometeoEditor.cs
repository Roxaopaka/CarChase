using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(PrometeoCarController))]
[System.Serializable]
public class PrometeoEditor : Editor{

  enum displayFieldType {DisplayAsAutomaticFields, DisplayAsCustomizableGUIFields}
  displayFieldType DisplayFieldType;

  private PrometeoCarController prometeo;
  private SerializedObject SO;

  //RION EDITED
  private SerializedProperty boss;
  //
  //
  //CAR SETUP
  //
  //
  private SerializedProperty maxSpeed;
  private SerializedProperty maxReverseSpeed;
  private SerializedProperty accelerationMultiplier;
  private SerializedProperty maxSteeringAngle;
  private SerializedProperty steeringSpeed;
  private SerializedProperty brakeForce;
  private SerializedProperty brakeDecelerationForce;
  private SerializedProperty steeringResponseMultiplier;
  private SerializedProperty steeringAssist;
  private SerializedProperty decelerationMultiplier;
  private SerializedProperty sidewaysGripRecovery;
  private SerializedProperty handbrakeDriftMultiplier;
  private SerializedProperty driftBuildSpeed;
  private SerializedProperty driftRecoverySpeed;
  private SerializedProperty frontDriftGripLoss;
  private SerializedProperty rearDriftGripLoss;
  private SerializedProperty driftYawAssist;
  private SerializedProperty driftForwardAssist;
  private SerializedProperty bodyMassCenter;
  //
  //
  //WHEELS VARIABLES
  //
  //
  private SerializedProperty frontLeftMesh;
  private SerializedProperty frontLeftCollider;
  private SerializedProperty frontRightMesh;
  private SerializedProperty frontRightCollider;
  private SerializedProperty rearLeftMesh;
  private SerializedProperty rearLeftCollider;
  private SerializedProperty rearRightMesh;
  private SerializedProperty rearRightCollider;
  //
  //
  //PARTICLE SYSTEMS' VARIABLES
  //
  //
  private SerializedProperty useEffects;
  private SerializedProperty RLWParticleSystem;
  private SerializedProperty RRWParticleSystem;
  private SerializedProperty RLWTireSkid;
  private SerializedProperty RRWTireSkid;
  //
  //
  //SPEED TEXT (UI) VARIABLES
  //
  //
  private SerializedProperty useUI;
  private SerializedProperty carSpeedText;
  //
  //
  //SPEED TEXT (UI) VARIABLES
  //
  //
  private SerializedProperty useSounds;
  private SerializedProperty carEngineSound;
  private SerializedProperty tireScreechSound;
  //
  //
  //TOUCH CONTROLS VARIABLES
  //
  //
  private SerializedProperty useTouchControls;
  private SerializedProperty throttleButton;
  private SerializedProperty reverseButton;
  private SerializedProperty turnRightButton;
  private SerializedProperty turnLeftButton;
  private SerializedProperty handbrakeButton;

  private void OnEnable(){
    prometeo = (PrometeoCarController)target;
    SO = new SerializedObject(target);

    maxSpeed = SO.FindProperty("maxSpeed");
    maxReverseSpeed = SO.FindProperty("maxReverseSpeed");
    accelerationMultiplier = SO.FindProperty("accelerationMultiplier");
    maxSteeringAngle = SO.FindProperty("maxSteeringAngle");
    steeringSpeed = SO.FindProperty("steeringSpeed");
    brakeForce = SO.FindProperty("brakeForce");
    brakeDecelerationForce = SO.FindProperty("brakeDecelerationForce");
    steeringResponseMultiplier = SO.FindProperty("steeringResponseMultiplier");
    steeringAssist = SO.FindProperty("steeringAssist");
    decelerationMultiplier = SO.FindProperty("decelerationMultiplier");
    sidewaysGripRecovery = SO.FindProperty("sidewaysGripRecovery");
    handbrakeDriftMultiplier = SO.FindProperty("handbrakeDriftMultiplier");
    driftBuildSpeed = SO.FindProperty("driftBuildSpeed");
    driftRecoverySpeed = SO.FindProperty("driftRecoverySpeed");
    frontDriftGripLoss = SO.FindProperty("frontDriftGripLoss");
    rearDriftGripLoss = SO.FindProperty("rearDriftGripLoss");
    driftYawAssist = SO.FindProperty("driftYawAssist");
    driftForwardAssist = SO.FindProperty("driftForwardAssist");
    bodyMassCenter = SO.FindProperty("bodyMassCenter");

    frontLeftMesh = SO.FindProperty("frontLeftMesh");
    frontLeftCollider = SO.FindProperty("frontLeftCollider");
    frontRightMesh = SO.FindProperty("frontRightMesh");
    frontRightCollider = SO.FindProperty("frontRightCollider");
    rearLeftMesh = SO.FindProperty("rearLeftMesh");
    rearLeftCollider = SO.FindProperty("rearLeftCollider");
    rearRightMesh = SO.FindProperty("rearRightMesh");
    rearRightCollider = SO.FindProperty("rearRightCollider");

    useEffects = SO.FindProperty("useEffects");
    RLWParticleSystem = SO.FindProperty("RLWParticleSystem");
    RRWParticleSystem = SO.FindProperty("RRWParticleSystem");
    RLWTireSkid = SO.FindProperty("RLWTireSkid");
    RRWTireSkid = SO.FindProperty("RRWTireSkid");

    useUI = SO.FindProperty("useUI");
    carSpeedText = SO.FindProperty("carSpeedText");

    useSounds = SO.FindProperty("useSounds");
    carEngineSound = SO.FindProperty("carEngineSound");
    tireScreechSound = SO.FindProperty("tireScreechSound");

    useTouchControls = SO.FindProperty("useTouchControls");
    throttleButton = SO.FindProperty("throttleButton");
    reverseButton = SO.FindProperty("reverseButton");
    turnRightButton = SO.FindProperty("turnRightButton");
    turnLeftButton = SO.FindProperty("turnLeftButton");
    handbrakeButton = SO.FindProperty("handbrakeButton");

    boss = SO.FindProperty("boss");

  }

  public override void OnInspectorGUI(){

    SO.Update();

    GUILayout.Space(25);
    GUILayout.Label("CAR SETUP", EditorStyles.boldLabel);
    GUILayout.Space(10);
    //
    //
    //CAR SETUP
    //
    //
    //
    maxSpeed.intValue = EditorGUILayout.IntSlider("Max Speed:", maxSpeed.intValue, 20, 5000);
    maxReverseSpeed.intValue = EditorGUILayout.IntSlider("Max Reverse Speed:", maxReverseSpeed.intValue, 10, 120);
    accelerationMultiplier.floatValue = EditorGUILayout.Slider("Acceleration Multiplier:", accelerationMultiplier.floatValue, 0.1f, 1000f);
    maxSteeringAngle.intValue = EditorGUILayout.IntSlider("Max Steering Angle:", maxSteeringAngle.intValue, 10, 45);
    steeringSpeed.floatValue = EditorGUILayout.Slider("Steering Speed:", steeringSpeed.floatValue, 0.1f, 1f);
    brakeForce.intValue = EditorGUILayout.IntSlider("Brake Force:", brakeForce.intValue, 100, 5000);
    brakeDecelerationForce.floatValue = EditorGUILayout.Slider("Brake Deceleration Force:", brakeDecelerationForce.floatValue, 1f, 80f);
    steeringResponseMultiplier.floatValue = EditorGUILayout.Slider("Steering Response:", steeringResponseMultiplier.floatValue, 1f, 5f);
    steeringAssist.floatValue = EditorGUILayout.Slider("Steering Assist:", steeringAssist.floatValue, 0f, 10f);
    decelerationMultiplier.intValue = EditorGUILayout.IntSlider("Deceleration Multiplier:", decelerationMultiplier.intValue, 1, 10);
    sidewaysGripRecovery.floatValue = EditorGUILayout.Slider("Sideways Grip Recovery:", sidewaysGripRecovery.floatValue, 0f, 30f);
    handbrakeDriftMultiplier.intValue = EditorGUILayout.IntSlider("Drift Multiplier:", handbrakeDriftMultiplier.intValue, 1, 10);
    driftBuildSpeed.floatValue = EditorGUILayout.Slider("Drift Build Speed:", driftBuildSpeed.floatValue, 0.5f, 8f);
    driftRecoverySpeed.floatValue = EditorGUILayout.Slider("Drift Recovery Speed:", driftRecoverySpeed.floatValue, 0.5f, 100f);
    frontDriftGripLoss.floatValue = EditorGUILayout.Slider("Front Drift Grip Loss:", frontDriftGripLoss.floatValue, 0f, 1f);
    rearDriftGripLoss.floatValue = EditorGUILayout.Slider("Rear Drift Grip Loss:", rearDriftGripLoss.floatValue, 0f, 1f);
    driftYawAssist.floatValue = EditorGUILayout.Slider("Drift Yaw Assist:", driftYawAssist.floatValue, 0f, 8f);
    driftForwardAssist.floatValue = EditorGUILayout.Slider("Drift Forward Assist:", driftForwardAssist.floatValue, 0f, 20f);
    EditorGUILayout.PropertyField(bodyMassCenter, new GUIContent("Mass Center of Car: "));

    //
    //
    //WHEELS
    //
    //

    GUILayout.Space(25);
    GUILayout.Label("WHEELS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    EditorGUILayout.PropertyField(frontLeftMesh, new GUIContent("Front Left Mesh: "));
    EditorGUILayout.PropertyField(frontLeftCollider, new GUIContent("Front Left Collider: "));

    EditorGUILayout.PropertyField(frontRightMesh, new GUIContent("Front Right Mesh: "));
    EditorGUILayout.PropertyField(frontRightCollider, new GUIContent("Front Right Collider: "));

    EditorGUILayout.PropertyField(rearLeftMesh, new GUIContent("Rear Left Mesh: "));
    EditorGUILayout.PropertyField(rearLeftCollider, new GUIContent("Rear Left Collider: "));

    EditorGUILayout.PropertyField(rearRightMesh, new GUIContent("Rear Right Mesh: "));
    EditorGUILayout.PropertyField(rearRightCollider, new GUIContent("Rear Right Collider: "));

    //
    //
    //EFFECTS
    //
    //

    GUILayout.Space(25);
    GUILayout.Label("EFFECTS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    useEffects.boolValue = EditorGUILayout.BeginToggleGroup("Use effects (particle systems)?", useEffects.boolValue);
    GUILayout.Space(10);

        EditorGUILayout.PropertyField(RLWParticleSystem, new GUIContent("Rear Left Particle System: "));
        EditorGUILayout.PropertyField(RRWParticleSystem, new GUIContent("Rear Right Particle System: "));

        EditorGUILayout.PropertyField(RLWTireSkid, new GUIContent("Rear Left Trail Renderer: "));
        EditorGUILayout.PropertyField(RRWTireSkid, new GUIContent("Rear Right Trail Renderer: "));

    EditorGUILayout.EndToggleGroup();

    

    //
    //
    //UI
    //
    //

    GUILayout.Space(25);
    GUILayout.Label("UI", EditorStyles.boldLabel);
    GUILayout.Space(10);

    useUI.boolValue = EditorGUILayout.BeginToggleGroup("Use UI (Speed text)?", useUI.boolValue);
    GUILayout.Space(10);

        EditorGUILayout.PropertyField(carSpeedText, new GUIContent("Speed Text (UI): "));

    EditorGUILayout.EndToggleGroup();

    //
    //
    //SOUNDS
    //
    //

    GUILayout.Space(25);
    GUILayout.Label("SOUNDS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    useSounds.boolValue = EditorGUILayout.BeginToggleGroup("Use sounds (car sounds)?", useSounds.boolValue);
    GUILayout.Space(10);

        EditorGUILayout.PropertyField(carEngineSound, new GUIContent("Car Engine Sound: "));
        EditorGUILayout.PropertyField(tireScreechSound, new GUIContent("Tire Screech Sound: "));

    EditorGUILayout.EndToggleGroup();

    //
    //
    //TOUCH CONTROLS
    //
    //

    GUILayout.Space(25);
    GUILayout.Label("TOUCH CONTROLS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    useTouchControls.boolValue = EditorGUILayout.BeginToggleGroup("Use touch controls (mobile devices)?", useTouchControls.boolValue);
    GUILayout.Space(10);

        EditorGUILayout.PropertyField(throttleButton, new GUIContent("Throttle Button: "));
        EditorGUILayout.PropertyField(reverseButton, new GUIContent("Brakes/Reverse Button: "));
        EditorGUILayout.PropertyField(turnLeftButton, new GUIContent("Turn Left Button: "));
        EditorGUILayout.PropertyField(turnRightButton, new GUIContent("Turn Right Button: "));
        EditorGUILayout.PropertyField(handbrakeButton, new GUIContent("Handbrake Button: "));

    EditorGUILayout.EndToggleGroup();


    GUILayout.Space(25);
    GUILayout.Label("BOSS", EditorStyles.boldLabel);
    GUILayout.Space(10);
    EditorGUILayout.PropertyField(boss, new GUIContent("Boss Object: "));

    //END

    GUILayout.Space(40);
    SO.ApplyModifiedProperties();

  }

}
