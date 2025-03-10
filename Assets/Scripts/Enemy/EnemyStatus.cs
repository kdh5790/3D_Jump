using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private int health;

    private bool isInvicibility = false;
    private bool isDie = false;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Start()
    {
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void OnDamaged(int damage)
    {
        if (isInvicibility || isDie) return;

        health = Mathf.Max(health - damage, 0);

        if (health <= 0 )
        {
            Die();
        }

        StartCoroutine(DamageFlash());
        StartCoroutine(InvicibilityCoroutine(0.5f));
    }

    private IEnumerator InvicibilityCoroutine(float duration)
    {
        isInvicibility = true;

        yield return new WaitForSeconds(duration);

        isInvicibility = false;
    }

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

    private void Die()
    {
        isDie = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
        GetComponent<EnemyAI>().StopNavMesh();
    }
}
