using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LeverTrap : MonoBehaviour, ILeverActionable
{
    [SerializeField] private Transform trapTransform; // 함정 역할을 할 오브젝트
    [SerializeField] private Lever lever;
    public Lever Lever => lever;

    private BoxCollider boxCollider;
    private NavMeshObstacle navMeshObstacle;


    private void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.enabled = false;
        navMeshObstacle.carving = false;
        navMeshObstacle.enabled = false;
    }

    // 레버 작동 시 실행된 행동이 모두 끝났을 때 호출 
    public void EndLeverAction()
    {
        lever.InitLeverRotation();
    }

    // 레버 작동 시 호출 
    public void StartLeverAction()
    {
        StartCoroutine(ActiveTrap());
    }

    // 함정 활성화
    private IEnumerator ActiveTrap()
    {
        boxCollider.enabled = true; // 데미지 판정을 위해 콜라이더 활성화

        // 시작 스케일, 목표 스케일, 지속시간, 실행 후 지난 시간 변수 초기화
        Vector3 startScale = trapTransform.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        float duration = 0.5f;
        float elapsedTime = 0f;

        // navMesh에 감지 될 수 있도록 활성화 후
        // 몬스터가 비켜 갈 수 있게 carving 활성화
        navMeshObstacle.enabled = true;
        navMeshObstacle.carving = true;

        // elapsedTime이 duation 보다 커질 때까지 반복
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // 시간 경과에 따른 보간 비율 계산
            trapTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        elapsedTime = 0f;

        boxCollider.enabled = false;
        navMeshObstacle.carving = false;
        navMeshObstacle.enabled = false;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trapTransform.localScale = Vector3.Lerp(targetScale, startScale, t);
            yield return null;
        }


        EndLeverAction();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // 플레이어나 적이 함정 콜라이더가 활성화 된 상태에서 안에 있다는게 감지된다면 지속적으로 데미지 부여
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.OnDamaged(20);
            }
            // 플레이어나 적에게 IDamageable 인터페이스가 존재 하지 않을 때의 예외 처리
            else
            {
                Debug.LogError("IDamageable 인터페이스를 찾지 못했습니다.");
            }
        }
    }
}
