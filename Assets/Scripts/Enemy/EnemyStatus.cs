using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private int health; // 적 체력

    private bool isInvicibility = false; // 무적 상태
    private bool isDie = false; // 사망 상태

    private EnemyAI enemyAi; 
    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers; // 데미지 피격 시 색 변경을 위한 오브젝트의 메쉬들

    private void Start()
    {
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        enemyAi = GetComponent<EnemyAI>();
    }

    public void OnDamaged(int damage)
    {
        // 무적상태거나 사망상태라면 리턴
        if (isInvicibility || isDie) return;

        // 최대값을 0 으로 설정하여 체력이 0 아래로 내려가지 않도록 함
        health = Mathf.Max(health - damage, 0);

        if (health <= 0 )
        {
            Die();
        }

        StartCoroutine(DamageFlash());
        StartCoroutine(InvicibilityCoroutine(0.5f));
    }

    // 무적 상태 코루틴 (duration : 지속시간)
    private IEnumerator InvicibilityCoroutine(float duration)
    {
        isInvicibility = true;

        yield return new WaitForSeconds(duration);

        isInvicibility = false;
    }

    // 피격 시 메쉬 색 변경 코루틴
    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1, 0.6f, 0.6f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }

    // 사망
    private void Die()
    {
        isDie = true;
        animator.SetTrigger("Die");

        // navmesh 정지
        enemyAi.StopNavMesh();

        // 애니메이션이 충분히 재생되고 난 뒤에 파괴되도록 2초뒤 파괴
        Destroy(gameObject, 2f);
    }
}
