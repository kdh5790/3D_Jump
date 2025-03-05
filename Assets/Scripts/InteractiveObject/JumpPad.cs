using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    Coroutine rayCheckCorountine;
    [SerializeField] private LayerMask playerLayer;
    private bool isScaling;
    private bool canJump;

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
                canJump = true;

                if (!isScaling)
                    StartCoroutine(JumpPadScaleChange(new Vector3(1, 0.1f, 1)));
            }
            else
            {
                canJump = false;

                if (!isScaling)
                    StartCoroutine(JumpPadScaleChange(new Vector3(1, 1, 1)));
            }

            yield return new WaitForSeconds(0.1f);
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
