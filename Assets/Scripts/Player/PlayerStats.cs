using System;
using System.Collections;
using UnityEngine;

// 데미지를 입을 수 있는 오브젝트에게 붙혀줄 인터페이스
public interface IDamageable
{
    void OnDamaged(int damage);
}

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100; // 최대 체력
    private int currentHealth; // 현재 체력
    private bool isInvicibility = false; // 무적 상태

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100; // 최대 스태미너
    [SerializeField] private float staminaDecreaseRate = 10f; // 스태미너 감소 배율
    [SerializeField] private float staminaRecoveryRate = 5f; // 스태미너 회복 배율
    [SerializeField] private float staminaRecoveryTime = 1f; // 사용 후 회복이 시작되기까지 시간
    private Coroutine staminaCoroutine; // 스태미너 회복/중지를 위한 코루틴 변수
    private bool isRecoveringStamina = false; // 현재 스태미너 회복 상태
    private float currentStamina; // 현재 스태미너
    public float CurrentStamina { get { return currentStamina; } }

    private PlayerController playerContoller;
    private SkinnedMeshRenderer[] meshRenderers;

    // 체력, 스태미너 값 변경 시 UI도 같이 변경되도록 실행할 action
    public Action<float> healthUIUpdateAction;
    public Action<float> staminaUIUpdateAction;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        playerContoller = Player.Instance.controller;
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Update()
    {
        // 현재 대쉬 중 && 플레이어가 이동 상태 && 스태미너가 0보다 큼 
        if (playerContoller.isSprint && playerContoller.moveState == MoveState.Move && currentStamina > 0)
        {
            // 현재 스태미너 회복중 || 스태미너 코루틴이 할당되어 있을 때
            if (isRecoveringStamina || staminaCoroutine != null)
            {
                // 코루틴을 중지 시키고 
                StopCoroutine(staminaCoroutine);
                isRecoveringStamina = false;
                staminaCoroutine = null;
            }
            // 스태미너 감소 코루틴 대입 후 실행
            staminaCoroutine = StartCoroutine(DecreaseStamina());
        }

        // 스태미너 회복중이 아니고 현재 스태미너가 최대 스태미너보다 낮을 때
        else if (!isRecoveringStamina && currentStamina < maxStamina)
        {
            if (staminaCoroutine != null)
            {
                StopCoroutine(staminaCoroutine);
            }

            // 스태미너 회복 코루틴 실행
            staminaCoroutine = StartCoroutine(RecoverStamina());
            isRecoveringStamina = true;
        }
    }

    // 스태미너 감소 코루틴
    private IEnumerator DecreaseStamina()
    {
        // 대쉬 상태라면
        while (currentStamina > 0 && playerContoller.isSprint && playerContoller.moveState == MoveState.Move)
        {
            // 스태미너 감소 속도에 맞춰 지속적으로 스태미너 감소
            currentStamina -= staminaDecreaseRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);

            // 스태미너 UI 업데이트
            staminaUIUpdateAction(currentStamina / maxStamina);

            // 스태미너가 0이라면 대쉬 중지
            if (currentStamina <= 0)
            {
                playerContoller.isSprint = false;
            }

            yield return null;
        }
    }

    // 스태미너 회복 코루틴
    private IEnumerator RecoverStamina()
    {
        // 스태미너 회복 대기 시간동안 정지
        yield return new WaitForSeconds(staminaRecoveryTime);

        // 스태미너가 모두 회복 될 때 까지 반복(중간에 대쉬를 한다면 Update함수에서 회복 함수를 중지 시킴)
        while (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);

            staminaUIUpdateAction(currentStamina / maxStamina);

            yield return null;
        }

        isRecoveringStamina = false;
    }

    // 플레이어가 데미지를 받았을 때 실행
    public void OnDamaged(int damage)
    {
        // 무적 상태라면 데미지를 받지 않고 리턴
        if (isInvicibility) return;

        currentHealth = (int)MathF.Max(0, currentHealth - damage);
        healthUIUpdateAction((float)currentHealth / (float)maxHealth);

        StartCoroutine(DamageFlash());
        ApplyInvicibility(0.5f);
    }

    // 체력 회복
    public void HealHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthUIUpdateAction((float)currentHealth / (float)maxHealth);
    }

    // 스태미너 회복
    public void HealStamina(int amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        staminaUIUpdateAction(currentStamina / maxStamina);

        // 스태미너 회복 후 스태미너가 감소하지 않는 현상 해결하기 위해 코루틴 변수 초기화
        if (staminaCoroutine != null)
        {
            StopCoroutine(staminaCoroutine);
            staminaCoroutine = null;
        }
        isRecoveringStamina = false;
    }

    // 외부에서 불러 올 무적 적용 함수
    public void ApplyInvicibility(float duration)
    {
        StartCoroutine(InvicibilityCoroutine(duration));
    }

    // 메쉬 색 변경 코루틴
    private IEnumerator DamageFlash()
    {
        Color[] colors = new Color[meshRenderers.Length];

        // 메쉬의 원래 색상들 저장
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            colors[i] = meshRenderers[i].material.color;
        }

        // 메쉬에 빨간색 색상 적용
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1, 0.3f, 0.3f);
        }

        yield return new WaitForSeconds(0.1f);

        // 원래 색상으로 되돌림
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = colors[i];
        }
    }

    // 무적 적용 코루틴
    private IEnumerator InvicibilityCoroutine(float duration)
    {
        isInvicibility = true;

        yield return new WaitForSeconds(duration);

        isInvicibility = false;
    }

    // 장비 변경 시 스탯 변경 함수
    public void SetEquipmentStatus(int equipHealth, float equipStamina)
    {
        maxHealth = equipHealth;
        maxStamina = equipStamina;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;

        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;

        staminaUIUpdateAction(currentStamina / maxStamina);
        healthUIUpdateAction((float)currentHealth / (float)maxHealth);
    }
}
