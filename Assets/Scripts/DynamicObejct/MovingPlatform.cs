using System.Collections;
using UnityEngine;

// 레버로 작동하는 오브젝트들에게 넣어줄 인터페이스
public interface ILeverActionable
{
    void StartLeverAction();
    void EndLeverAction();

    Lever Lever { get; }
}

public class MovingPlatform : MonoBehaviour, ILeverActionable
{
    [SerializeField] private Vector3 startPos; // 시작 위치
    [SerializeField] private Vector3 endPos; // 도달 위치
    [SerializeField] private Transform boxPivot; // 플레이어가 현재 플랫폼 위에 있는지 확인 하기 위해 생성해둔 플랫폼 중심점 오브젝트
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Lever lever;
    public Lever Lever => lever;


    private Vector3 boxSize = new Vector3(5f, 2f, 5f); // 플레이어 감지 할 박스 크기

    private void Start()
    {
        startPos = transform.position;
    }

    public void StartLeverAction()
    {
        StartCoroutine(MoveToTargetPos());
    }

    public void EndLeverAction()
    {
        lever.InitLeverRotation();
    }

    // 플랫폼을 지정된 위치로 이동 시키는 코루틴
    private IEnumerator MoveToTargetPos()
    {
        // 플랫폼의 위치가 첫 위치와 가깝다면 target을 endPos로 설정 / 끝 위치와 가깝다면 target을 startPos로 설정
        Vector3 target = Vector3.Distance(transform.position, startPos) < Vector3.Distance(transform.position, endPos) ? endPos : startPos;

        float moveSpeed = 5f;

        // 플레이어의 부모를 플랫폼으로 설정
        Player.Instance.transform.SetParent(transform);

        // 플랫폼이 목표 위치에 도달 할 때 까지 반복
        while (Vector3.Distance(transform.position, target) >= 0.01f)
        {
            // 플랫폼을 목표 위치로 이동시킴
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            // 플레이어가 플랫폼에서 벗어난게 감지됐다면 설정해둔 플레이어의 부모를 해제해줌
            if (!IsPlayerOnPlatform())
                Player.Instance.transform.SetParent(null);

            // 다시 들어왔다면 플레이어의 부모 재설정
            else
                Player.Instance.transform.SetParent(transform);


            yield return null;
        }

        transform.position = target;

        Player.Instance.transform.SetParent(null);

        EndLeverAction();
    }

    // 플랫폼 위에 플레이어가 있는지 확인 할 함수
    private bool IsPlayerOnPlatform()
    {
        // boxPivot 위치에서 boxSize 만큼의 크기로 Player Layer를 가진 오브젝트들 감지
        Collider[] colliders = Physics.OverlapBox(boxPivot.position, boxSize, Quaternion.identity, playerLayer);

        // 플레이어가 감지됐다면 true 감지되지 않았다면 false 반환
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    // 감지 범위 디버깅을 위한 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPivot.position, boxSize);
    }
}
