using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chase,
    Attack
}

public class Enemy_AI : MonoBehaviour
{
    public static EnemyState enemyState;

    private NavMeshAgent myNavMeshAgent;
    [SerializeField] private Transform _target;
    private float chaseRange = 10f;
    private float hitRange = 2f;
    private float distanceToTarget;
    private float hitAnimationTime = 3.4f;

    private Animator animator;
   
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        distanceToTarget = Mathf.Infinity; // begin with player out of range
        animator = GetComponent<Animator>();
        enemyState = EnemyState.Idle;
    }


    void Update()
    {
        distanceToTarget = CalculateDistanceToTarget(_target);

        if (distanceToTarget <= hitRange)
        {
            enemyState = EnemyState.Attack;
        }
        else if (distanceToTarget > hitRange && distanceToTarget <= chaseRange)
        {
            enemyState = EnemyState.Chase;
        }
        else
        {
            enemyState = EnemyState.Idle;
        }

        Debug.Log("Enemy state: " + enemyState);

        switch (enemyState)
        {
            case EnemyState.Idle:
                myNavMeshAgent.isStopped = true;
                myNavMeshAgent.ResetPath();
                animator.SetBool("Chase", false);
                break;
            case EnemyState.Chase:
                myNavMeshAgent.SetDestination(_target.position);
                myNavMeshAgent.isStopped = false;
                animator.SetBool("Chase", true);
                break;
            case EnemyState.Attack:
                myNavMeshAgent.isStopped = true;
                myNavMeshAgent.ResetPath();
                animator.SetBool("Chase", false);
                HitPlayer();
                break;
        }



/*        distanceToTarget = Vector3.Distance(target.position, transform.position); // distance between player & this enemy

        Debug.Log("Distance: " + distanceToTarget);*/

        // chase if player in range

        /*        if (distanceToTarget <= hitRange)
                {
                    myNavMeshAgent.isStopped = true;
                    myNavMeshAgent.SetDestination(myNavMeshAgent.destination);
                    hitPlayer();
                }

                else if (distanceToTarget <= chaseRange)
                {
                    myNavMeshAgent.SetDestination(target.position);
                    myNavMeshAgent.isStopped = false;
                    animator.SetBool("Chase", true);
                }
                else
                {
                    myNavMeshAgent.isStopped = true;
                    animator.SetBool("Chase", false);
                }*/


/*        if (distanceToTarget <= chaseRange)
        {
            myNavMeshAgent.SetDestination(target.position);
            animator.SetBool("Chase", true);
            //myNavMeshAgent.isStopped = false;

            if (distanceToTarget <= hitRange)
            {
                myNavMeshAgent.isStopped = true;
                hitPlayer();
            }

            else
            {
                myNavMeshAgent.isStopped = false;
            }
        }
        else
        {
            animator.SetBool("Chase", false);
            myNavMeshAgent.isStopped = true;
        }*/


    }

    private void HitPlayer() 
    {
        // damage player
        animator.SetTrigger("Attack");
        //myNavMeshAgent.isStopped = true;
        //yield return new WaitForSeconds(hitAnimationTime);
    }

    private float CalculateDistanceToTarget(Transform target)
    {
        return Vector3.Distance(target.position, transform.position);
    }


}
