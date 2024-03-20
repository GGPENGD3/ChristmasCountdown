using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{

    public LayerMask groundLayer, playerLayer;
    private NavMeshAgent agent;
    private Animator anim;
    public List<Transform> playerTransform;
    public List<Transform> waypoints;

    Transform currentDestination;
    bool walkPointSet;
    int waypointIndex;
    public bool walking, chasing;
    
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        walking = true;
        waypointIndex = Random.Range(0,waypoints.Count);
        currentDestination = waypoints[waypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChasePlayer(Transform player)
    {
        agent.SetDestination(player.position);
    }


}
