using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractive
{
    [SerializeField] private Transform leverHandle;
    [SerializeField] private ObjectInfo info;

    private ILeverActionable[] leverActionObject;
    private Vector3 firstRot = new Vector3(-40f, 0, 0);
    private Vector3 lastRot = new Vector3(90f, 0, 0);

    public bool canInteract;
    public bool isLeverAction;

    private void Start()
    {
        leverActionObject = GetComponentsInParent<ILeverActionable>();

        if(leverActionObject.Length == 0)
            leverActionObject = GetComponentsInChildren<ILeverActionable>();

        if (leverActionObject.Length == 0)
            Debug.LogError("레버와 상호작용 할 오브젝트를 찾지 못했습니다. 설정을 확인해주세요.");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract && !isLeverAction)
        {
            StartCoroutine(RotateLever(firstRot, lastRot));

            foreach (var lever in leverActionObject)
                lever.StartLeverAction();

            isLeverAction = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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

    private IEnumerator RotateLever(Vector3 startRot, Vector3 endRot)
    {
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            Vector3 currentRot = Vector3.Lerp(startRot, endRot, elapsedTime / duration);

            leverHandle.localRotation = Quaternion.Euler(currentRot);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        leverHandle.localRotation = Quaternion.Euler(endRot);
    }

    public void InitLeverRotation()
    {
        StartCoroutine(RotateLever(lastRot, firstRot));
        isLeverAction = false;
    }

    public ObjectInfo GetObjectInfo() => info;
}
