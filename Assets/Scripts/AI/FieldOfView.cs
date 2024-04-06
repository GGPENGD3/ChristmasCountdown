using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool seePlayer1, seePlayer2, seePlayer3, seePlayer4;
    public MonsterAI monsterAI;

    private void Start()
    {
   
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheckOnePlayer()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    Debug.Log("Player " + target.name + " is within FOV");
                    if (target.CompareTag("P1"))
                    {
                        
                    }
                    seePlayer1 = true;
                }
                  
                else
                    seePlayer1 = false;
            }
            else
                seePlayer1 = false;
        }
        else if (seePlayer1)
            seePlayer1 = false;
    }

    private void FieldOfViewCheck()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, radius, targetMask);

        foreach (Collider targetCollider in targetsInViewRadius)
        {
            Transform target = targetCollider.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    // If player is within view and not obstructed, do something (e.g., set canSeePlayer flag, call a method)
                    //Debug.Log("Player " + target.name + " is within field of view!");
                    if (target.CompareTag("P1"))
                    {
                        monsterAI.seePlayer1 = true;
                        seePlayer1 = true;
                    }
                    else
                        monsterAI.seePlayer1 = false;
                        seePlayer1 = false;
                    if (target.CompareTag("P2"))
                    {
                        monsterAI.seePlayer2 = true;
                        seePlayer2 = true;
                    }
                    else
                    monsterAI.seePlayer2 = false;
                    seePlayer2 = false;
                    if (target.CompareTag("P3"))
                    {
                        monsterAI.seePlayer3 = true;
                        seePlayer3 = true;
                    }
                    else
                    monsterAI.seePlayer3 = false;
                    seePlayer3 = false;
                    if (target.CompareTag("P4"))
                    {
                        monsterAI.seePlayer4 = true;
                        seePlayer4 = true;
                    }
                    else
                        monsterAI.seePlayer4 = false;
                        seePlayer4 = false;
                }
   
            }
            //else
            //    Debug.Log("Player " + target.name + " is NOT within field of view!");
            //monsterAI.seePlayer1 = false;
            //monsterAI.seePlayer2 = false;
            //monsterAI.seePlayer3 = false;
            //monsterAI.seePlayer4 = false;
            //seePlayer1 = false;
            //seePlayer2 = false;
            //seePlayer3 = false;
            //seePlayer4 = false;
        }
    }
}