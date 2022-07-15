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
    [SerializeField] private EnemyState currentEnemyState = EnemyState.Idle;
    [SerializeField] private Transform _target;
    [SerializeField] private float chaseRange;
    
    private NavMeshAgent myNavMeshAgent;
    private float hitRange = 2f;
    private float distanceToTarget;
    private float hitAnimationTime = 3.4f;

    private Animator animator;
   
    private void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
        distanceToTarget = Mathf.Infinity; // begin with player out of range
        animator = GetComponent<Animator>();
        SetState(EnemyState.Idle);
    }
    
    private void Update()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
        }
    }

    private void UpdateIdleState()
    {
        distanceToTarget = CalculateDistanceToTarget(_target);
        if (distanceToTarget <= hitRange)
        {
            SetState(EnemyState.Attack);
        }
        else if (distanceToTarget > hitRange && distanceToTarget <= chaseRange)
        {
            SetState(EnemyState.Chase);
        }
    }

    private void UpdateChaseState()
    {
        // Update movement
        myNavMeshAgent.SetDestination(_target.position);
        myNavMeshAgent.isStopped = false;
        animator.SetBool("Chase", true);
        
        // Check range
        distanceToTarget = CalculateDistanceToTarget(_target);
        if (distanceToTarget <= hitRange)
        {
            SetState(EnemyState.Attack);
        }
        else if (distanceToTarget > chaseRange)
        {
            SetState(EnemyState.Idle);
        }
    }
    
    private void UpdateAttackState()
    {
        // Do nothing?
    }

    private IEnumerator CoPerformAttack()
    {
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(hitAnimationTime);
        SetState(EnemyState.Chase);
    }
    
    
    private void SetState(EnemyState argNewState)
    {
        if (currentEnemyState == argNewState)
        {
            // Don't do anything if state is the same.
            return;
        }

        currentEnemyState = argNewState;
        
        // Handle ENTERING new states.
        switch (currentEnemyState)
        {
            case EnemyState.Idle:
                myNavMeshAgent.isStopped = true;
                myNavMeshAgent.ResetPath();
                animator.SetBool("Chase", false);
                break;
            case EnemyState.Chase:
                myNavMeshAgent.isStopped = false;
                animator.SetBool("Chase", true);
                break;
            case EnemyState.Attack:
                myNavMeshAgent.isStopped = true;
                myNavMeshAgent.ResetPath();
                animator.SetBool("Chase", false);
                
                // Perform the attack.
                StartCoroutine(CoPerformAttack());
                break;
        }
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
