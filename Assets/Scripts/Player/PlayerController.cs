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

    public bool canMove = true;
    public bool canLook = true;
    public bool isSprint = false;

    private Rigidbody rigid;
    private Animator anim;

    public MoveState moveState;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.inventoryUI.OpenUI();
        }
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
        anim.SetBool("IsGrounded", IsGrounded());
    }

    void Move()
    {
        Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
        dir *= moveSpeed;
        dir.y = rigid.velocity.y;

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

        cameraContainer.localEulerAngles = new Vector3(-camCurXrot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
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
        if (context.phase == InputActionPhase.Started && moveState == MoveState.Move)
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

    bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 1f, groundLayerMask))
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
}
