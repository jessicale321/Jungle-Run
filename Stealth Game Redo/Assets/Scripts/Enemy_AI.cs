using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    private NavMeshAgent myNavMeshAgent;
    [SerializeField] private Transform target;
    private float chaseRange = 10f;
    private float hitRange = 1.5f;
    private float distanceToTarget;

   
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        distanceToTarget = Mathf.Infinity; // begin with player out of range

    }


    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position); // distance between player & this enemy

        // chase if player in range
        if (distanceToTarget <= chaseRange) {
            myNavMeshAgent.SetDestination(target.position);
            GetComponent<Animator>().SetBool("Chase", true);
            myNavMeshAgent.isStopped = false;
            
            if (distanceToTarget <= hitRange) 
            {
                myNavMeshAgent.isStopped = true;
                hitPlayer();
            }
        }
        else 
        {
            GetComponent<Animator>().SetBool("Chase", false);
            myNavMeshAgent.isStopped = true;
        }
    }

    public void hitPlayer() 
    {
        // damage player
        GetComponent<Animator>().SetTrigger("Attack");
    }


}
