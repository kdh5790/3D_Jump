using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveState
{
    Idle,
    Move
}

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 3f;
    public float equipmentSpeed = 0f;
    public float increasedSpeed = 0f;
    public float jumpPower = 80f;
    private Vector2 movementInput;
    public LayerMask groundLayerMask; // 바닥 감지를 위한 레이어

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    public float lookSensitivity; // 마우스 감도
    private float minXLook = -30f;
    private float maxXLook = 35f;
    private float camCurXrot;
    private Vector2 mouseDelta;

    [Header("Hanging")]
    [SerializeField] private GameObject hangRayPivot; // 매달릴 벽을 체크할 레이의 위치 피벗
    [SerializeField] private LayerMask hangLayer; // 벽타기 가능한 벽 레이어
    private bool isHanging = false; // 현재 벽타기 중인지 확인
    private bool canHang = false; // 벽타기가 가능한 상태인지 확인
    private bool canClimb = false; // 벽타기 중 벽을 올라탈 수 있는 상태인지 확인
    private Coroutine climbCoroutine;
    private Coroutine climbCheckCoroutine;
    private Coroutine canHangCheckCoroutine;


    [Header("State")]
    public bool canMove = true; // 이동 가능 여부
    public bool canLook = true; // 시점 회전 가능 여부
    public bool isSprint = false; // 대쉬 여부
    private bool isGrounded = true;

    private Rigidbody rigid;
    private Animator anim;

    public MoveState moveState;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // 커서 숨김 설정
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        if (canMove)
            Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();

        // 현재 입력값에 따라 파라미터 값 설정
        anim.SetFloat("Horizontal", movementInput.x, 0.2f, Time.deltaTime);
        anim.SetFloat("Vertical", movementInput.y, 0.2f, Time.deltaTime);

        isGrounded = IsGrounded();
        anim.SetBool("IsGrounded", isGrounded);

        // 현재 공중에 있다면 벽타기 가능 여부 체크 코루틴 실행
        if (!isGrounded && canHangCheckCoroutine == null)
            canHangCheckCoroutine = StartCoroutine(CanHangCheck());
    }

    void Move()
    {
        // 현재 벽타기 중이라면 앞뒤 이동을 상하 이동으로 변경
        Vector3 dir = ((isHanging ? transform.up : transform.forward) * movementInput.y) + (transform.right * movementInput.x);
        dir *= moveSpeed;
        if (!isHanging)
            dir.y = rigid.velocity.y;
        else
            dir.x = 0f;

        rigid.velocity = dir;
    }

    public void Stop()
    {
        moveState = MoveState.Idle;
        movementInput = Vector2.zero;
        isSprint = false;
    }

    void CameraLook()
    {
        camCurXrot += mouseDelta.y * lookSensitivity;
        camCurXrot = Mathf.Clamp(camCurXrot, minXLook, maxXLook);


        if (!isHanging)
        {
            cameraContainer.localEulerAngles = new Vector3(-camCurXrot, 0, 0);
            transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
        }
        else
            cameraContainer.localEulerAngles += new Vector3(-camCurXrot - cameraContainer.localEulerAngles.x, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && canMove)
        {
            movementInput = context.ReadValue<Vector2>();

            // 입력된 값에 따라 속도 변경
            if (movementInput.y == 0 && (movementInput.x > 0f || movementInput.x < 0f)) // 좌우 이동
                moveSpeed = increasedSpeed + equipmentSpeed - 1f;

            else if (movementInput.y < 0) // 뒤로 이동
                moveSpeed = increasedSpeed + equipmentSpeed - 2f;

            else // 앞으로 이동
                moveSpeed = increasedSpeed + equipmentSpeed;

            if (isSprint)
                moveSpeed += 5f;

            moveState = MoveState.Move;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Stop();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && moveState == MoveState.Move && !isHanging)
        {
            if (!isSprint)
            {
                moveSpeed += 5f;
                isSprint = true;
            }
            else
            {
                moveSpeed -= 5f;
                isSprint = false;
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnHang(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && canHang)
        {
            HangCharacter();
        }
    }

    public void OnClimb(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && canClimb)
        {
            if (climbCoroutine == null)
                climbCoroutine = StartCoroutine(ClimbToPosition(hangRayPivot.transform.position + transform.up));

            StopCoroutine(CanHangClimbCheck());
            climbCheckCoroutine = null;
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            UIManager.Instance.inventoryUI.OpenUI();
        }
    }

    bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 0.7f, groundLayerMask))
            return true;

        return false;
    }

    public IEnumerator JumpPadGroundedCheck()
    {
        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (IsGrounded())
            {
                canMove = true;
                yield break;
            }
            yield return null;
        }
    }

    // 속도 증가 물약 사용 시 실행되는 함수
    public void ApplySpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(duration));
    }

    // 지속시간 동안 속도 증가
    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        increasedSpeed = 3f;

        yield return new WaitForSeconds(duration);

        increasedSpeed = 0f;
    }

    // 벽타기 
    private void HangCharacter()
    {
        // 벽타기 실행 시 플레이어의 중력을 끔
        rigid.useGravity = false;
        isHanging = true;
        anim.SetBool("IsHanging", isHanging);
        StartCoroutine(CanHangClimbCheck());
    }

    // 벽타기 가능여부 체크
    private IEnumerator CanHangCheck()
    {
        float distance = 0.7f;

        while (!IsGrounded())
        {
            // 디버그용 Ray
            Debug.DrawRay(hangRayPivot.transform.position, transform.forward, Color.red, distance);

            // 매달릴 수 있는 벽이 감지됐다면
            if (Physics.Raycast(hangRayPivot.transform.position, transform.forward, distance, hangLayer))
            {
                canHang = true;
            }

            // 벽에 탄 상태에서 현재 타고있는 벽을 벗어난 경우
            else if (Physics.Raycast(hangRayPivot.transform.position, transform.forward, distance, hangLayer) == false && isHanging)
            {
                canHang = false;
                rigid.useGravity = true;
                isHanging = false;
                anim.SetBool("IsHanging", isHanging);
            }
            yield return new WaitForFixedUpdate();
        }

        canHang = false;

        canHangCheckCoroutine = null;
    }

    // 등반 가능 여부 체크
    private IEnumerator CanHangClimbCheck()
    {
        float waitTime = 0.1f;

        while (isHanging)
        {
            // 플레이어 위에 Ray를 발사하여 현재 감지되는 벽이 있는지 확인(등반 가능여부 확인용)
            Debug.DrawRay(hangRayPivot.transform.position + (Vector3.up * 1.5f), transform.forward, Color.red, 1f);
            if (!Physics.Raycast(hangRayPivot.transform.position + (Vector3.up * 1.5f), transform.forward, 1f, hangLayer) && !canClimb)
                canClimb = true;

            yield return new WaitForSeconds(waitTime);
        }

        canHangCheckCoroutine = null;
        yield break;
    }

    // 등반 애니메이션 실행
    private IEnumerator ClimbToPosition(Vector3 targetPosition)
    {
        // 플레이어 행동 제한
        canClimb = false;
        canMove = false;
        rigid.useGravity = false;

        // 애니메이션 재생
        anim.SetTrigger("Climb");

        // 플레이어의 위치를 수동으로 조절
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        startPosition = transform.position;
        targetPosition = transform.position + transform.forward;
        GetComponent<CapsuleCollider>().isTrigger = true;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // 플레이어 행동 제한 해제
        GetComponent<CapsuleCollider>().isTrigger = false;
        rigid.useGravity = true;
        anim.ResetTrigger("Climb");
        anim.SetBool("IsHanging", false);
        canHang = false;
        isHanging = false;
        canMove = true;
        climbCoroutine = null;
        climbCheckCoroutine = null;
        canClimb = false;
    }
}
