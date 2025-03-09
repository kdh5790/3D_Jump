using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaycastTrap : MonoBehaviour
{
    [SerializeField] private Transform rayPivot;
    [SerializeField] private GameObject trapObj;
    [SerializeField] private GameObject trapTarget;

    private bool isActive;

    private void Update()
    {
        Debug.DrawRay(rayPivot.position, transform.forward * 15f, Color.red);

        if(!isActive && Physics.Raycast(rayPivot.position, transform.forward * 15f, out RaycastHit hitInfo))
        {
            isActive = true;
            StartCoroutine(TrapActive());
        }
    }

    private IEnumerator TrapActive()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = trapObj.transform.position;

        while (elapsedTime < duration)
        {
            trapObj.transform.position = Vector3.Lerp(startPosition, trapTarget.transform.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        duration = 1f;

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
