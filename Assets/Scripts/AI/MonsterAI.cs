using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum AIState { Patrol, Investigate, Chase, Capture };
    public AIState currentState;
    public LayerMask groundLayer, playerLayer;
    private NavMeshAgent ai;
    private Animator anim;
    public List<Transform> playerTransform;
    public List<Transform> playersInRange;
    public List<Transform> waypoints;

    public float walkSpeed = 5f;
    public float chaseSpeed = 8f;
    public float minIdleTime = 0f, maxIdleTime = 0.5f;
    public float investigateTime = 2f;
    public float visionRange = 8f;
    public float visionAngle = 45f;
    public float catchDistance = 2f;

    float idleTime;
    public Transform currentDestination;
    public Transform investigatePoint;
    public Transform nearestPlayer;
    Vector3 dest;
    bool walkPointSet;
    int waypointIndex;
    public bool walking, chasing;

    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        walking = true;
        waypointIndex = Random.Range(0, waypoints.Count);
        currentDestination = waypoints[waypointIndex];
        currentState = AIState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        LineOfSight();
        if (playersInRange!=null && currentState!= AIState.Chase || playersInRange!=null && currentState !=AIState.Capture)
        {
            
            currentState = AIState.Chase;
        } 

        else if (investigatePoint != null)
        {
            currentState = AIState.Investigate;
        }
        else currentState = AIState.Patrol;


        switch (currentState)
        {
            case AIState.Patrol:

                Patrol();
                break;
            case AIState.Investigate:
                Investigate(investigatePoint);
                break;

            case AIState.Chase:

                break;
            case AIState.Capture:

                break;

        }
    }

    void ChasePlayer(Transform player)
    {
        ai.SetDestination(player.position);
    }

    void Patrol()
    {

        if (walking)
        {
            dest = currentDestination.position;
            ai.destination = dest;
            ai.speed = walkSpeed;

            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                ai.speed = 0;
                walking = false;
                StartCoroutine(IdleTimer());

            }
        }


    }

    IEnumerator IdleTimer()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        waypointIndex = Random.Range(0, waypoints.Count);
        currentDestination = waypoints[waypointIndex];
    }



    public void Investigate(Transform destination)
    {
        walking = true;
        if (walking)
        {
            currentDestination = destination;
            dest = currentDestination.position;
            ai.destination = dest;
            ai.speed = walkSpeed;

            if (ai.remainingDistance <= ai.stoppingDistance)
            {
                ai.speed = 0;
                walking = false;
                StartCoroutine(InvestigateTimer());

            }

        }
    }

    IEnumerator InvestigateTimer()
    {

        yield return new WaitForSeconds(investigateTime);
        if (nearestPlayer!=null)
        {
            Debug.Log("go to nearest player");
        }
        else
        walking = true;
        waypointIndex = Random.Range(0, waypoints.Count);
        currentDestination = waypoints[waypointIndex];
        investigatePoint = null;
        currentState = AIState.Patrol;
    }

    void LineOfSight()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, visionRange, playerLayer);

        foreach (Collider player in players)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
           
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
             //90 degree vision cone
            if (angleToPlayer <= visionAngle / 2f)
            {
                RaycastHit hit;
                Debug.DrawRay(transform.position, directionToPlayer, Color.red);
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Transform playerTrans = hit.collider.transform;
                        if (!playersInRange.Contains(playerTrans))
                        {
                            playersInRange.Add(playerTrans);
                        }
                    }
                }
            }
        }
          
    }

    void SetChaseTarget()
    {
        if (playersInRange.Count <= 1)
        {
            nearestPlayer = playersInRange[0];
        }
        
        
    }
        
}
