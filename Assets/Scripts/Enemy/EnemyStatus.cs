using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    [SerializeField] private int health;

    private bool isInvicibility = false;
    private bool isDie = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnDamaged(int damage)
    {
        if (isInvicibility || isDie) return;

        health = Mathf.Max(health - damage, 0);

        if (health <= 0 )
        {
            Die();
        }

        StartCoroutine(InvicibilityCoroutine(0.5f));
    }

    private IEnumerator InvicibilityCoroutine(float duration)
    {
        isInvicibility = true;

        yield return new WaitForSeconds(duration);

        isInvicibility = false;
    }

    private void Die()
    {
        isDie = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
        GetComponent<EnemyAI>().StopNavMesh();
    }
}
