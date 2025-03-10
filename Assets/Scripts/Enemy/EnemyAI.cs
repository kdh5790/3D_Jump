using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Move,
    Attack
}

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    private float playerDistance;

    private AIState state;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    void Start()
    {
        target = Player.Instance.transform;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        SetState(AIState.Idle);
    }

    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, Player.Instance.transform.position);

        animator.SetBool("IsMove", state != AIState.Idle);

        if(playerDistance < 20f)
        {
            SetState(AIState.Move);
            navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
    }

    private void SetState(AIState _state)
    {
        state = _state;

        switch (state)
        {
            case AIState.Idle:
                navMeshAgent.isStopped = true;
                break;
            case AIState.Move:
                navMeshAgent.isStopped = false;
                break;
            case AIState.Attack:
                navMeshAgent.isStopped = false;
                break;
        }
    }

    public void StopNavMesh()
    {
        navMeshAgent.isStopped = true;
    }
}
