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
    private Transform target;

    private float playerDistance; // 플레이어와 거리
    private float detectionRange = 20f; // 플레이어를 발견할 거리
    private float attackRange = 3f; // 공격 사정거리
    private float attackCooldown = 2f; // 공격 쿨타임

    private bool canAttack = true; // 공격 가능 여부

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
        // 플레이어와 적의 거리 계산
        playerDistance = Vector3.Distance(transform.position, Player.Instance.transform.position);

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
            // navmesh 경로를 플레이어로 지속해서 설정
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
            // isStopped : navmesh 정지 여부
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
        // 공격 불가능 상태(쿨타임) 이라면 코루틴 중지
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
