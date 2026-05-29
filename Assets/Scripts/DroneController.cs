using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour
{

    public GameObjManager boss;
    private Rigidbody rb;
    private NavMeshAgent droneAgent;   //Create a public reference so that the component, "navMeshAgent" can be used in the code

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        // Prevent Rigidbody from fighting the NavMeshAgent
        rb.isKinematic = true;

        droneAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
    Vector3 userLocation = boss.getUserLocation();
    Vector3 droneDestination = userLocation + Vector3.up * 30f;

    droneAgent.SetDestination(droneDestination);

    }
}
