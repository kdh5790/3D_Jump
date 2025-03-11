using System.Collections;
using UnityEngine;

// 플레이어가 감지하여 설명을 띄워줄 오브젝트들에게 넣어줄 인터페이스
public interface IInteractive
{
    ObjectInfo GetObjectInfo();
}

public class JumpPad : MonoBehaviour, IInteractive
{
    Coroutine rayCheckCorountine;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private bool canJumpMove; // 점프대를 이용하여 점프 중 이동 가능한지 

    private Player player;
    private bool isScaling; // 현재 스케일 조정 중인지 확인
    private bool canJump; // 점프 가능 상태인지 확인

    public ObjectInfo info;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump && player != null)
        {
            // 점프 중 이동 가능한 점프대인지 확인하여 플레이어 행동 제한
            player.controller.canMove = canJumpMove ? true : false;
            
            // 점프시킬 힘
            Vector3 power = transform.up.normalized * 200f;

            // 플레이어를 점프대가 바라보고 있는 상태 기준 위로 힘을 가해줌
            player.GetComponent<Rigidbody>().AddForce(transform.up * 200f, ForceMode.Impulse);

            if (!canJumpMove)
                StartCoroutine(Player.Instance.controller.JumpPadGroundedCheck());
        }
    }

    // 플레이어가 설정해둔 Trigger에 입장 했을 때만 Ray 검사
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rayCheckCorountine = StartCoroutine(RayCheckCoroutine());
        }
    }

    // 플레이어가 Trigger에서 나갔다면 Ray 검사 중지
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText(string.Empty);
            player = null;
            StopCoroutine(rayCheckCorountine);
            rayCheckCorountine = null;
        }
    }

    // Ray 감지 코루틴
    public IEnumerator RayCheckCoroutine()
    {
        while (true)
        {
            RaycastHit hit;

            // 점프대가 바라보는 방향 기준 위 방향으로 Ray 검사 실행
            if (Physics.Raycast(transform.position, transform.up, out hit, 3f, playerLayer))
            {
                // 플레이어가 감지됐다면 점프 가능 상태로 변경
                if (hit.transform.TryGetComponent(out Player _player))
                {
                    player = _player;
                    canJump = true;

                    UIManager.Instance.descriptionUI.SetInteractionDescriptionText("Space키를 입력해 높이 점프 할 수 있습니다.");

                    if (!isScaling)
                        StartCoroutine(JumpPadScaleChange(new Vector3(1, 0.1f, 1)));
                }
            }
            else
            {
                canJump = false;
                player = null;
                UIManager.Instance.descriptionUI.SetInteractionDescriptionText(string.Empty);
                if (!isScaling)
                    StartCoroutine(JumpPadScaleChange(new Vector3(1, 1, 1)));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // 점프패드의 y값 크기 조절
    private IEnumerator JumpPadScaleChange(Vector3 targetScale)
    {
        isScaling = true;

        Vector3 startScale = transform.localScale;
        float duration = 0.5f;
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        isScaling = false;
        transform.localScale = targetScale;
    }

    public ObjectInfo GetObjectInfo() => info;

    // 방향 확인 디버깅 함수
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 power = transform.up.normalized * 25f;
        Gizmos.DrawLine(transform.position, transform.position + power);
    }
}
