using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    Coroutine rayCheckCorountine;
    [SerializeField] private LayerMask playerLayer;

    private Player player;
    private bool isScaling;
    private bool canJump;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canJump && player != null)
            player.GetComponent<Rigidbody>().AddForce(Vector2.up * 200f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rayCheckCorountine = StartCoroutine(StartRayCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            StopCoroutine(rayCheckCorountine);
            rayCheckCorountine = null;
        }
    }

    private IEnumerator StartRayCoroutine()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.up, out hit, 3f, playerLayer))
            {
                if (hit.transform.TryGetComponent(out Player _player))
                {
                    player = _player;
                    canJump = true;

                    if (!isScaling)
                        StartCoroutine(JumpPadScaleChange(new Vector3(1, 0.1f, 1)));
                }
            }
            else
            {
                canJump = false;
                player = null;  
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
}
