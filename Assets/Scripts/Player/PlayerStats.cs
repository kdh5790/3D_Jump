using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaDecreaseRate = 10f; // ���¹̳� ���� ����
    [SerializeField] private float staminaRecoveryRate = 5f; // ���¹̳� ȸ�� ����
    [SerializeField] private float staminaRecoveryTime = 1f; // ��� �� ȸ���� ���۵Ǳ���� �ð�
    private Coroutine staminaCoroutine;
    private bool isRecoveringStamina = false;
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } }

    PlayerController playerContoller;

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
            if (isRecoveringStamina)
            {
                StopCoroutine(staminaCoroutine);
                isRecoveringStamina = false;
                staminaCoroutine = StartCoroutine(DecreaseStamina());
            }
            if (staminaCoroutine == null)
            {
                staminaCoroutine = StartCoroutine(DecreaseStamina());
            }
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
        while(currentStamina > 0 && playerContoller.isSprint && playerContoller.moveState == MoveState.Move)
        {
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);

            if (currentStamina <= 0)
            {
                playerContoller.isSprint = false;
            }

            yield return null;
        }
    }

    private IEnumerator RecoverStamina()
    {
        Debug.Log("Recover");
        yield return new WaitForSeconds(staminaRecoveryTime);

        while(currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
            yield return null;
        }

        isRecoveringStamina = false;
    }
}
