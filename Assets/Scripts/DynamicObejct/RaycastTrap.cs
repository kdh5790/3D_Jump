using System.Collections;
using UnityEngine;

public class RaycastTrap : MonoBehaviour
{
    [SerializeField] private Transform rayPivot; // 오브젝트를 감지할 Ray의 시작 위치
    [SerializeField] private GameObject trapObj; // 오브젝트 감지 시 이동시킬 함정 오브젝트
    [SerializeField] private GameObject trapTarget; // 함정이 도달할 위치

    private BoxCollider boxCollider;
    private bool isActive;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void Update()
    {
        // 플레이어와의 거리가 10 보다 작을 때만 Ray 검사
        if(Vector3.Distance(Player.Instance.transform.position, rayPivot.position) < 10f && !isActive && Physics.Raycast(rayPivot.position, transform.forward * 15f, out RaycastHit hitInfo))
        {
            isActive = true;
            StartCoroutine(TrapActive());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.TryGetComponent(out PlayerStats stats))
        {
            // 플레이어와 충돌 시 뒤로 힘을 가한 후 데미지 부여
            collision.rigidbody.AddForce(-collision.transform.forward * 120f, ForceMode.VelocityChange);
            stats.OnDamaged(50);
        }
    }

    // 함정 활성화 코루틴
    private IEnumerator TrapActive()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = trapObj.transform.position;

        yield return new WaitForSeconds(0.2f);

        // 데미지 부여를 위한 collider 활성화
        boxCollider.enabled = true;

        while (elapsedTime < duration)
        {
            trapObj.transform.position = Vector3.Lerp(startPosition, trapTarget.transform.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = false;
        elapsedTime = 0f;
        duration = 1f;

        // 함정을 다시 원위치로 되돌림
        Vector3 tempPos = trapObj.transform.position;

        yield return new WaitForSeconds(1f);

        while (elapsedTime < duration)
        {
            while (elapsedTime < duration)
            {
                trapObj.transform.position = Vector3.Lerp(tempPos, startPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        isActive = false;
    }
}
