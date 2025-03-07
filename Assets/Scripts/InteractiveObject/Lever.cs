using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractive
{
    [SerializeField] private Transform leverHandle;
    [SerializeField] private ObjectInfo info;
    
    private ILeverActionable leverActionObject;
    private Vector3 firstRot = new Vector3(-40f, 0, 0);
    private Vector3 lastRot = new Vector3(90f, 0, 0);

    public bool canInteract;
    public bool isLeverAction;

    private void Start()
    {
        leverActionObject = GetComponentInParent<ILeverActionable>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && canInteract && !isLeverAction)
        {
            StartCoroutine(OnInteract());
            leverActionObject.StartLeverAction();
            isLeverAction = true;
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

    public IEnumerator InitInteractState()
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            Vector3 currentRot = Vector3.Lerp(lastRot, firstRot, elapsedTime / duration);

            leverHandle.localRotation = Quaternion.Euler(currentRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leverHandle.localRotation = Quaternion.Euler(firstRot);

        isLeverAction = false;
    }

    public ObjectInfo GetObjectInfo()
    {
        return info;
    }
}
