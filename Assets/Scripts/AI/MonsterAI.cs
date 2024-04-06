using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum AIState { Patrol, Investigate, Chase, Capture };
    public AIState currentState;
    public LayerMask groundLayer, playerLayer;
   
    private NavMeshAgent agent;
    public List<Transform> playerTransform;
    public List<Transform> playersInRange;
    public List<Transform> investigatePoints;
    public List<Transform> waypoints;
    public Transform centrePoint;
    public float range;
    public Transform raycastPos;
    public float walkSpeed = 3f;
    public float chaseSpeed = 5f;
    public float minIdleTime = 0f, maxIdleTime = 0.5f;
    public float investigateTime = 2f;
    public float visionRange = 10f;
    public float visionAngle = 45f;
    public float catchDistance = 2f;
    public float soundDetectionRange = 10f;
    public float LOSTimer;
    public float LOSInterval;
    float idleTime;
    public Transform currentDestination;
    public Vector3 investigatePoint;
    public Transform closestPlayer;
    public Transform caughtPlayer;
    Vector3 dest;
    int waypointIndex;
    public bool walking, investigating, chasing, capture;
    public bool seePlayer1, seePlayer2, seePlayer3, seePlayer4;
    public Transform possessPosition;
    public Animator anim;
    public WhoIsMonster whoIsMonsterScript;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        walking = true;
        waypointIndex = Random.Range(0, waypoints.Count);
        currentDestination = waypoints[waypointIndex];
        currentState = AIState.Patrol;
        agent.autoBraking = true;

    }

    // Update is called once per frame
    void Update()
    {
        //LOS2();
        CheckPlayer();
        if (playersInRange!=null)
        {
            closestPlayer = ReturnNearestPlayer();
            
        } 

        if (closestPlayer!=null)
        {
            if (Vector3.Distance(closestPlayer.position, transform.position) >= 1)
            {
                currentState = AIState.Chase;
                chasing = true;
                walking = false;
            }
         
              

        }


        else if (investigating)
        {
            currentState = AIState.Investigate;
            chasing = false;
            walking = true;
        }
        //else currentState = AIState.Patrol;


        switch (currentState)
        {
            case AIState.Patrol:

                Patrol2();
                break;
            case AIState.Investigate:
                Investigate(investigatePoint);
                break;

            case AIState.Chase:
                //ReturnNearestPlayer();
                ChasePlayer();
                break;
            case AIState.Capture:
                //transform.LookAt(closestPlayer);
                agent.updateRotation = false;
                agent.speed = 0;
                agent.transform.LookAt(closestPlayer);
                if (!capture)
                {
                    capture = true;
                    StartCoroutine(Capture());
                }
              
                break;

        }
        if (walking)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
        }
        if (chasing)
        {
            if (walking)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Run", true);
            }
        }
    }

    void ChasePlayer()
    {
        if (closestPlayer!=null)
        {
            agent.speed = chaseSpeed;
            agent.ResetPath();
            agent.SetDestination(closestPlayer.position);

            //if (Vector3.Distance(closestPlayer.position, transform.position) <= 4f)
            //if (agent.remainingDistance <= 0.5f)
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);
            //Debug.Log(distanceToPlayer);
            if (distanceToPlayer <= 2.5f) 
            {
                agent.speed = 0;
                agent.ResetPath();
                agent.isStopped = true;
                //agent.updateRotation = false;
                //agent.transform.LookAt(closestPlayer);
                //agent.transform.LookAt(closestPlayer.position);
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                StartCoroutine(Capture());

            }
         

        }

       
    }

    void Patrol2()
    {
        if (agent.remainingDistance<=agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position,range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.red, 2.0f);
                agent.ResetPath();
                agent.SetDestination(point);
            }
        }
    }

    bool RandomPoint (Vector3 Center, float range, out Vector3 result)
    {
        Vector3 randomPoint = Center + Random.insideUnitSphere * range;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("attacking player");
        if (closestPlayer!=null)
        {
            Debug.Log("caught player");
            StartCoroutine(Capture());
           
        }
    }

    IEnumerator Capture()
    {
        if (!capture)
        {
            capture = true;
            Debug.Log("capturing");
            FindObjectOfType<AudioManager>().Play("sfx", "player_die");
            anim.SetTrigger("Attack");
            //anim.SetTrigger("Possess");
            yield return new WaitForSeconds(2f);
            //closestPlayer.gameObject.transform.LookAt(transform.position);
            closestPlayer.GetComponent<FPS_Controller>().playerCanMove = false;
            anim.SetTrigger("Possess");

            yield return new WaitForSeconds(3f);
            PlayerToMonster();
        }

    }

    void Patrol()
    {

        if (walking)
        {
            dest = currentDestination.position;
            agent.destination = dest;
            agent.speed = walkSpeed;
            
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.speed = 0;
                walking = false;
                StartCoroutine(IdleTimer());

            }
        }


    }

    IEnumerator IdleTimer()
    {
        anim.SetBool("Walk", false);
        anim.SetBool("Run", false);
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        yield return new WaitForSeconds(idleTime);
        walking = true;
        waypointIndex = Random.Range(0, waypoints.Count);
        //waypointIndex = (waypointIndex+1) % waypoints.Count;
        currentDestination = waypoints[waypointIndex];
    }

    public void Investigate(Vector3 destination)
    {
        walking = true;
        if (walking)
        {
          

            agent.destination = destination;
            agent.speed = walkSpeed;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.speed = 0;
                walking = false;
                StartCoroutine(InvestigateTimer());
               
            }

        }
    }

    IEnumerator InvestigateTimer()
    {

        yield return new WaitForSeconds(investigateTime);
        if (closestPlayer!=null)
        {
            Debug.Log("go to nearest player");
        }
        else
        walking = true;
        waypointIndex = Random.Range(0, waypoints.Count);
        currentDestination = waypoints[waypointIndex];
        investigating = false;
        currentState = AIState.Patrol;
    }

    void LineOfSight()
    {

        Collider[] players = Physics.OverlapSphere(transform.position, visionRange, playerLayer);
        List<Transform> newTrackedPlayers = new List<Transform>();
        foreach (Collider player in players)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            Transform playerTransform = player.transform;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
             //90 degree vision cone
            if (angleToPlayer <= visionAngle / 2f)
            {
                RaycastHit hit;
                Debug.DrawRay(transform.position, directionToPlayer, Color.red);
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, visionRange))
                {
                    if (hit.collider!=null)
                    {
                        if (hit.collider.CompareTag("P1"))
                        {
                            seePlayer1 = true;
                 
                        }
                        if (hit.collider.CompareTag("P2"))
                        {
                            seePlayer2 = true;
                        }
                        if (hit.collider.CompareTag("P3"))
                        {
                            seePlayer3 = true;
                        }
                        if (hit.collider.CompareTag("P4"))
                        {
                            seePlayer4 = true;
                        }
                    }


         
                }
            }
        }
      
    }

    void LOS2()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, visionRange, playerLayer);

        foreach (Collider player in players)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer <= visionAngle / 2f)
            {
                // Check if there are obstacles between the enemy and the player
                RaycastHit hit;
                if (Physics.Raycast(raycastPos.position, directionToPlayer, out hit, visionRange))
                {
                    if (hit.collider.GetComponent<FPS_Controller>()!=null)
                    {
                        // Player is within vision range and cone
                        if (!playersInRange.Contains(player.transform))
                        {
                            playersInRange.Add(player.transform);
                        }
                      
                    }
                }
            }
        }
    }

   public Transform ReturnNearestPlayer() //check for all players in range, return nearest player
    {

        if (playersInRange.Count == 0)
        {
            //closestPlayer = null;
            return null;
        }

        Transform nearestPlayer = playersInRange[0];
        float closestDistance = Vector3.Distance(nearestPlayer.position, transform.position);
        
        foreach (Transform playerTransform in playersInRange)
        {
            float distance = Vector3.Distance(playerTransform.position, transform.position);
            if (distance < closestDistance)
            {
                nearestPlayer= playerTransform;
                closestDistance = distance;
            }
        }
        return nearestPlayer;

    }

    void CheckPlayer()
    {
        if (seePlayer1)
        {
            if (!playersInRange.Contains(playerTransform[0]))
            {
                playersInRange.Add(playerTransform[0]);
            }
        }
        if (!seePlayer1)
        {
            if (playersInRange != null)
            {
                if (playersInRange.Contains(playerTransform[0]))
                {
                    if (Vector3.Distance(transform.position, playerTransform[0].position) >= 6f)
                    {
                        playersInRange.Remove(playerTransform[0]);
                    }
                   
                }
            }

        }
        if (seePlayer2)
        {
            if (!playersInRange.Contains(playerTransform[1]))
            {
                playersInRange.Add(playerTransform[1]);
            }
        }
        if (!seePlayer2)
        {
            if (playersInRange.Contains(playerTransform[1]))
            {
                if (Vector3.Distance(transform.position, playerTransform[0].position) >= 6f)
                {
                    playersInRange.Remove(playerTransform[1]);
                }
            }
        }
        if (seePlayer3)
        {
            if (!playersInRange.Contains(playerTransform[2]))
            {
                playersInRange.Add(playerTransform[2]);
            }
        }
        if (!seePlayer3)
        {
            if (playersInRange.Contains(playerTransform[2]))
            {
                if (Vector3.Distance(transform.position, playerTransform[0].position) >= 6f)
                {
                    playersInRange.Remove(playerTransform[2]);
                }
            }
        }
        if (seePlayer4)
        {
            if (!playersInRange.Contains(playerTransform[3]))
            {
                playersInRange.Add(playerTransform[3]);
            }
        }
        if (!seePlayer4)
        {
            if (playersInRange.Contains(playerTransform[3]))
            {
                if (Vector3.Distance(transform.position, playerTransform[0].position) >= 6f)
                {
                    playersInRange.Remove(playerTransform[3]);
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            seePlayer1 = true;
            closestPlayer = other.transform;
        }
        if (other.CompareTag("P2"))
        {
            seePlayer2 = true;
            closestPlayer = other.transform;

        }
        if (other.CompareTag("P1"))
        {
            seePlayer3 = true;
            closestPlayer = other.transform;

        }
        if (other.CompareTag("P1"))
        {
            seePlayer4 = true;
            closestPlayer = other.transform;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            seePlayer1 = false;
            //closestPlayer = null;

        }
        if (other.CompareTag("P2"))
        {
            seePlayer2 = false;
            //closestPlayer = null;

        }
        if (other.CompareTag("P1"))
        {
            seePlayer3 = false;
            //closestPlayer = null;

        }
        if (other.CompareTag("P1"))
        {
            seePlayer4 = false;
            //closestPlayer = null;

        }
    }
    void ClearLOS()
    {
        LOSTimer += Time.deltaTime;
        if (LOSTimer >=LOSInterval)
        {
            LOSTimer = 0;
            seePlayer1 = false;
            seePlayer2 = false;
            seePlayer3 = false;
            seePlayer4 = false;
        }
    }

    #region AI Catch Player Code
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //YS pls insert code to play catch animation

    //    #region Setting which player to become the monster based on who the AI catched
    //    //if (collision.gameObject.tag == "P1")
    //    //{
    //    //    whoIsMonsterScript.currentMonsterPlayer = "P1";
    //    //    whoIsMonsterScript.ChangeMonster();
    //    //    Destroy(this.gameObject);
    //    //}
    //    //else if (collision.gameObject.tag == "P2")
    //    //{
    //    //    whoIsMonsterScript.currentMonsterPlayer = "P2";
    //    //}
    //    //else if(collision.gameObject.tag == "P3")
    //    //{
    //    //    whoIsMonsterScript.currentMonsterPlayer = "P3";
    //    //}
    //    //else if (collision.gameObject.tag == "P4")
    //    //{
    //    //    whoIsMonsterScript.currentMonsterPlayer = "P4";
    //    //}
    //    #endregion
    //}

    void PlayerToMonster()
    {
        if (closestPlayer.gameObject.tag == "P1")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P1";
            whoIsMonsterScript.ChangeMonster();
            closestPlayer.gameObject.transform.Find("Furby Collider?");
            Destroy(this.gameObject);
        }
        else if (closestPlayer.gameObject.tag == "P2")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P2";
            whoIsMonsterScript.ChangeMonster();
            Destroy(this.gameObject);
        }
        else if (closestPlayer.gameObject.tag == "P3")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P3";
            whoIsMonsterScript.ChangeMonster();
            Destroy(this.gameObject);
        }
        else if (closestPlayer.gameObject.tag == "P4")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P4";
            whoIsMonsterScript.ChangeMonster();
            Destroy(this.gameObject);
        }
    }
    #endregion
}
   