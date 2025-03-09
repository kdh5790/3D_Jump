using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float increasedSpeed = 0f;
    private float jumpPower = 80f;
    private Vector2 movementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    public float lookSensitivity;
    private float minXLook = -30f;
    private float maxXLook = 35f;
    private float camCurXrot;
    private Vector2 mouseDelta;

    [Header("Hanging")]
    [SerializeField] private GameObject hangRayPivot; // 매달릴 벽을 체크할 레이의 위치 피벗
    [SerializeField] private LayerMask hangLayer;
    private bool isHanging = false;
    private bool canHang = false;
    private bool canClimb = false;
    private Coroutine climbCoroutine;
    private Coroutine climbCheckCoroutine;
    private Coroutine canHangCheckCoroutine;


    [Header("State")]
    public bool canMove = true;
    public bool canLook = true;
    public bool isSprint = false;
    private bool isGrounded = true;

    private Rigidbody rigid;
    private Animator anim;

    public MoveState moveState;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
            CameraLook();

        anim.SetFloat("Horizontal", movementInput.x, 0.2f, Time.deltaTime);
        anim.SetFloat("Vertical", movementInput.y, 0.2f, Time.deltaTime);

        isGrounded = IsGrounded();
        anim.SetBool("IsGrounded", isGrounded);

        if (!isGrounded && canHangCheckCoroutine == null)
            canHangCheckCoroutine = StartCoroutine(CanHangCheck());
    }

    void Move()
    {
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

            if (movementInput.y == 0 && (movementInput.x > 0f || movementInput.x < 0f)) // 좌우 이동
                moveSpeed = 3f + increasedSpeed;

            else if (movementInput.y < 0) // 뒤로 이동
                moveSpeed = 4f + increasedSpeed;

            else // 앞으로 이동
                moveSpeed = 5f + increasedSpeed;

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

    public void ApplySpeedBoost(float duration)
    {
        StartCoroutine(SpeedBoostCoroutine(duration));
    }

    private IEnumerator SpeedBoostCoroutine(float duration)
    {
        increasedSpeed = 3f;

        yield return new WaitForSeconds(duration);

        increasedSpeed = 0f;
    }

    private void HangCharacter()
    {
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
            Debug.DrawRay(hangRayPivot.transform.position + (Vector3.up * 1.5f), transform.forward, Color.red, 1f);
            if (!Physics.Raycast(hangRayPivot.transform.position + (Vector3.up * 1.5f), transform.forward, 1f, hangLayer) && !canClimb)
                canClimb = true;

            yield return new WaitForSeconds(waitTime);
        }

        canHangCheckCoroutine = null;
        yield break;
    }

    private IEnumerator ClimbToPosition(Vector3 targetPosition)
    {
        canClimb = false;
        canMove = false;
        rigid.useGravity = false;

        anim.SetTrigger("Climb");

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
        Debug.Log($"{climbCoroutine == null} ???????");
    }
}
