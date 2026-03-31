using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
public class CarDriveMechs : MonoBehaviour
{
    public GameObject playerLocation;  //Create a public reference so that the AI can follow the player's location
    private NavMeshAgent navMeshAgent;   //Create a public reference so that the component, "navMeshAgent" can be used in the code
    private int speeds = 10;  //Set the initial speed of the AI to 4
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int hit = 0;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //Reference
        
        navMeshAgent.speed = speeds; //Set the initial speed of the AI to speeds (which is 4)
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.5f, gameObject.transform.position.z);

        if (hit != 1)
        {
            navMeshAgent.SetDestination(playerLocation.transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CAR"))
        {
            hit = 1;
        }
    }
}


/*using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class NavM : MonoBehaviour
{
    //This is the code for the AI Navigation

    
    public GameObject playerLocation;  //Create a public reference so that the AI can follow the player's location
    private NavMeshAgent navMeshAgent;   //Create a public reference so that the component, "navMeshAgent" can be used in the code
    private int speeds = 4;  //Set the initial speed of the AI to 4
    public PlayerController code; //Create a public reference so that the variable, "score" inside PlayerController's code can be used. The score is used to increase the AI's speed
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); //Reference
        
        navMeshAgent.speed = speeds; //Set the initial speed of the AI to speeds (which is 4)

    }

    // Update is called once per frame
    void Update()
    {
        //This if statement checks if the player is still alive, and then sets the AI to chase the player by tracking their location constantly
        if (playerLocation != null)
        {
            navMeshAgent.SetDestination(playerLocation.transform.position);
            
        }
        //This constantly updates the speed according to the score. The AI's speed increases by 0.5 for everytime the player gathers a coin 
        navMeshAgent.speed = (float)((code.score)/2+speeds);
        

        if (code.score == 8) //If the player gathers all 8 coins, destroy this object
        {
            Destroy(this.gameObject);
        }
        
    }
}
*/
