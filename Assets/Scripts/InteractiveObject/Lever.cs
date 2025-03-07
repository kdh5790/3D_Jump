using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractive
{
    [SerializeField] private Transform leverHandle;
    [SerializeField] private ObjectInfo info;

    private Vector3 firstRot = new Vector3(-40f, 0, 0);
    private Vector3 lastRot = new Vector3(90f, 0, 0);

    public bool canInteract;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && canInteract)
        {
            StartCoroutine(OnInteract());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private IEnumerator OnInteract()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            Vector3 currentRot = Vector3.Lerp(firstRot, lastRot, elapsedTime / duration);

            leverHandle.localRotation = Quaternion.Euler(currentRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leverHandle.localRotation = Quaternion.Euler(lastRot);
    }

    public ObjectInfo GetObjectInfo()
    {
        return info;
    }
}
