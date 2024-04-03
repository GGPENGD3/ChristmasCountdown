using System.Collections;
using System.Collections.Generic;
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

    public float walkSpeed = 5f;
    public float chaseSpeed = 8f;
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
    Vector3 dest;
    int waypointIndex;
    public bool walking, investigating, chasing;
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
        agent.autoBraking = false;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        LineOfSight();
        CheckPlayer();
        if (playersInRange!=null)
        {
            closestPlayer = ReturnNearestPlayer();
            
        } 

        if (closestPlayer!=null)
        {
            currentState = AIState.Chase;
            chasing = true;
            walking = false;

        }

        //else if (closestPlayer == null)
        //{
        //    currentState = AIState.Patrol;
        //    chasing = false;
        //    walking = true;
        //}
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
                ReturnNearestPlayer();
                ChasePlayer();
                break;
            case AIState.Capture:

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
  
            if (Vector3.Distance(closestPlayer.position, transform.position) <= 2)
            {
                agent.speed = 0;
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                StartCoroutine(Capture());
            }
            else
                agent.speed = chaseSpeed;
                agent.SetDestination(closestPlayer.position);

        }

       
    }

    void Patrol2()
    {
        if (agent.remainingDistance<=agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position,range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
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
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        closestPlayer.GetComponent<FPS_Controller>().playerCanMove = false;
        anim.SetTrigger("Possess");
       
        yield return new WaitForSeconds(3f);
        PlayerToMonster();
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
        //waypointIndex = Random.Range(0, waypoints.Count);
        waypointIndex = (waypointIndex+1) % waypoints.Count;
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
                    //if (hit.collider.CompareTag("Player"))
                    //{
                    //    Debug.Log("Saw player");
                    //    if (hit.collider.name == "Player 1")
                    //    {
                    //        seePlayer1 = true;

                    //    }
                    //    if (hit.collider.name == "Player 2")
                    //    {
                    //        seePlayer2 = true;
                    //    }
                    //    if (hit.collider.name == "Player 3")
                    //    {
                    //        seePlayer3 = true;
                    //    }
                    //    if (hit.collider.name == "Player 4")
                    //    {
                    //        seePlayer4 = true;
                    //    }
                    //}
                    //else 
                    //seePlayer1 = false;
                    //seePlayer2 = false;
                    //seePlayer3 = false;
                    //seePlayer4 = false;
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
            if (playersInRange!=null)
            {
                if (playersInRange.Contains(playerTransform[0]))
                {
                    playersInRange.Remove(playerTransform[0]);
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
                playersInRange.Remove(playerTransform[1]);
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
                playersInRange.Remove(playerTransform[2]);
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
                playersInRange.Remove(playerTransform[3]);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FPS_Controller>()!=null)
        {
            closestPlayer = other.transform;
            Debug.Log("detected player via trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FPS_Controller>() != null)
        {
            closestPlayer = null;
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
    private void OnCollisionEnter(Collision collision)
    {
        //YS pls insert code to play catch animation

        #region Setting which player to become the monster based on who the AI catched
        //if (collision.gameObject.tag == "P1")
        //{
        //    whoIsMonsterScript.currentMonsterPlayer = "P1";
        //    whoIsMonsterScript.ChangeMonster();
        //    Destroy(this.gameObject);
        //}
        //else if (collision.gameObject.tag == "P2")
        //{
        //    whoIsMonsterScript.currentMonsterPlayer = "P2";
        //}
        //else if(collision.gameObject.tag == "P3")
        //{
        //    whoIsMonsterScript.currentMonsterPlayer = "P3";
        //}
        //else if (collision.gameObject.tag == "P4")
        //{
        //    whoIsMonsterScript.currentMonsterPlayer = "P4";
        //}
        #endregion
    }

    void PlayerToMonster()
    {
        if (closestPlayer.gameObject.tag == "P1")
        {
            whoIsMonsterScript.currentMonsterPlayer = "P1";
            whoIsMonsterScript.ChangeMonster();
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
