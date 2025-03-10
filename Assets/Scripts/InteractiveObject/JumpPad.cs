using System.Collections;
using UnityEngine;

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
    private bool isScaling;
    private bool canJump;

    public ObjectInfo info;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump && player != null)
        {
            player.controller.canMove = canJumpMove ? true : false;
            Vector3 power = transform.up.normalized * 200f;
            Debug.Log(power);
            player.GetComponent<Rigidbody>().AddForce(transform.up * 200f, ForceMode.Impulse);

            if (!canJumpMove)
                StartCoroutine(Player.Instance.controller.JumpPadGroundedCheck());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rayCheckCorountine = StartCoroutine(RayCheckCoroutine());
        }
    }

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

    public IEnumerator RayCheckCoroutine()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, 3f, playerLayer))
            {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 power = transform.up.normalized * 25f;
        Gizmos.DrawLine(transform.position, transform.position + power);
    }
}
