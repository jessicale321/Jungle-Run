using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent myNavMeshAgent;
    public Transform target;
    public float chaseRange = 10f;
    public float hitRange = 1f;
    float distanceToTarget = Mathf.Infinity; // begin with player out of range
    // Start is called before the first frame update
    void Start()
    {
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position); // distance between player & this enemy

        // chase if player in range
        if (distanceToTarget <= chaseRange) {
            myNavMeshAgent.SetDestination(target.position);
        }

        if (distanceToTarget <= hitRange) {
            hitPlayer();
        }
    }

    public void hitPlayer() {
        // damage player
        SceneMngr.reloadLevel();
    }
}
