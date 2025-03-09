using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isInvicibility = false;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaDecreaseRate = 10f; // 스태미너 감소 배율
    [SerializeField] private float staminaRecoveryRate = 5f; // 스태미너 회복 배율
    [SerializeField] private float staminaRecoveryTime = 1f; // 사용 후 회복이 시작되기까지 시간
    private Coroutine staminaCoroutine;
    private bool isRecoveringStamina = false;
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }

    PlayerController playerContoller;

    public Action<float> healthUIUpdateAction;
    public Action<float> staminaUIUpdateAction;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        playerContoller = Player.Instance.controller;
    }

    void Update()
    {
        if (playerContoller.isSprint && playerContoller.moveState == MoveState.Move && currentStamina > 0)
        {
            if (isRecoveringStamina || staminaCoroutine != null)
            {
                StopCoroutine(staminaCoroutine);
                isRecoveringStamina = false;
                staminaCoroutine = null;
            }

            staminaCoroutine = StartCoroutine(DecreaseStamina());
        }
        else if (!isRecoveringStamina && currentStamina < maxStamina)
        {
            if (staminaCoroutine != null)
            {
                StopCoroutine(staminaCoroutine);
            }
            staminaCoroutine = StartCoroutine(RecoverStamina());
            isRecoveringStamina = true;
        }
    }

    private IEnumerator DecreaseStamina()
    {
        while (currentStamina > 0 && playerContoller.isSprint && playerContoller.moveState == MoveState.Move)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);

            staminaUIUpdateAction(currentStamina / maxStamina);

            if (currentStamina <= 0)
            {
                playerContoller.isSprint = false;
            }

            yield return null;
        }
    }

    private IEnumerator RecoverStamina()
    {
        yield return new WaitForSeconds(staminaRecoveryTime);

        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);

            staminaUIUpdateAction(currentStamina / maxStamina);

            yield return null;
        }

        isRecoveringStamina = false;
    }

    public void OnDamaged(int damage)
    {
        if(isInvicibility) return;

        currentHealth = (int)MathF.Max(0, currentHealth - damage);
        healthUIUpdateAction((float)currentHealth / (float)maxHealth);
    }

    public void HealHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthUIUpdateAction((float)currentHealth / (float)maxHealth);
    }

    public void HealStamina(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        staminaUIUpdateAction(currentStamina / maxStamina);

        if (staminaCoroutine != null)
        {
            StopCoroutine(staminaCoroutine);
            staminaCoroutine = null;
        }
        isRecoveringStamina = false;
    }

    public void ApplyInvicibility(float duration)
    {
        StartCoroutine(InvicibilityCoroutine(duration));
    }

    private IEnumerator InvicibilityCoroutine(float duration)
    {
        isInvicibility = true;

        yield return new WaitForSeconds(duration);

        isInvicibility = false;
    }
}
