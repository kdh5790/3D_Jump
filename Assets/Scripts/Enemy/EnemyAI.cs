using System.Collections;
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
    private float detectionRange = 20f;
    private float attackRange = 3f;
    private float attackCooldown = 2f;

    private bool canAttack = true;

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

        Debug.Log(playerDistance);

        animator.SetBool("IsMove", state != AIState.Idle);

        if (playerDistance < attackRange && canAttack)
        {
            SetState(AIState.Attack);
            StartCoroutine(Attack());
        }
        else if (playerDistance < attackRange && !canAttack)
        {
            SetState(AIState.Idle);
        }
        else if (playerDistance < detectionRange)
        {
            SetState(AIState.Move);
            navMeshAgent.SetDestination(Player.Instance.transform.position);
        }
        else
        {
            SetState(AIState.Idle);
        }
    }

    private void SetState(AIState _state)
    {
        if (state == _state) return;

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
                navMeshAgent.isStopped = true;
                break;
        }
    }

    private IEnumerator Attack()
    {
        if (!canAttack) yield break;

        canAttack = false;

        animator.SetTrigger("Attack");

        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.OnDamaged(10);
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void StopNavMesh()
    {
        navMeshAgent.speed = 0f;
        navMeshAgent.isStopped = true;
    }
}
