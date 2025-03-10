using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    NavMeshAgent nmAgent;

    void Start()
    {
        target = Player.Instance.transform;

        nmAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        nmAgent.SetDestination(target.position);
    }

    public void StopNavMesh()
    {
        nmAgent.isStopped = true;
    }
}
