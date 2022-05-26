using UdonSharp;
using UnityEngine;
using UnityEngine.AI;
using VRC.SDKBase;
using VRC.Udon;

public class EnemyController : UdonSharpBehaviour
{
    [HideInInspector] public NavMeshAgent Agent;
    public BaseState Patrol;
    public BaseState Chase;
    // Used to prevent null references to player; does nothing.
    public BaseState NoPlayerState;
    [SerializeField] private BaseState currentState;
    public VRCPlayerApi LP;
    // Where the player will be sent to if they are caught by the enemy.
    public Transform LocalPlayerSpawn;
    public Animator Animator;
    // Player will only be detected if they are within this radius.
    [SerializeField] private float playerDetectionRadius;
    // Yellow alert will display if player is within this radius.
    public float PlayerAlertRadius;
    public GameObject YellowAlert;
    public GameObject RedAlert;

    private void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        LP = Networking.LocalPlayer;
        if(LP != null)
        {
            currentState = Patrol;
        }
        else
        {
            currentState = NoPlayerState;
        }
        currentState.Construct();
    }

    private void Update()
    {
        currentState.Action();
        currentState.Transition();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Destruct();
        currentState = newState;
        currentState.Construct();
    }

    public bool PlayerDetected()
    {
        Vector3 playerHeadPos = LP.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        if (!Physics.Linecast(transform.position, playerHeadPos) 
            && Vector3.Distance(transform.position, playerHeadPos) <= playerDetectionRadius)
        {
            return true;
        }
        return false;
    }
}
