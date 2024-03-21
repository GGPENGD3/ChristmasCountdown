using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum AIState {Patrol,Investigate,Chase,Capture};
    public AIState currentState;
    public LayerMask groundLayer, playerLayer;
    private NavMeshAgent ai;
    private Animator anim;
    public List<Transform> playerTransform;
    public List<Transform> waypoints;

    public float walkSpeed = 5f;
    public float chaseSpeed=8f;
    public float minIdleTime=0f, maxIdleTime=0.5f;

    float idleTime;
    public Transform currentDestination;
    Vector3 dest;
    bool walkPointSet;
    int waypointIndex;
    public bool walking, chasing;
    
    void Start()
    {
        ai=GetComponent<NavMeshAgent>();
        walking = true;
        waypointIndex = Random.Range(0,waypoints.Count);
       currentDestination = waypoints[waypointIndex];
        currentState = AIState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Investigate:

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
        idleTime = Random.Range(minIdleTime,maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        waypointIndex=Random.Range(0,waypoints.Count);
        currentDestination = waypoints[waypointIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
