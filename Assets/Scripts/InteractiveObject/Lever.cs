using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractive
{
    [SerializeField] private Transform leverHandle; // 레버 손잡이
    [SerializeField] private ObjectInfo info;

    private ILeverActionable[] leverActionObject;
    private Vector3 firstRot = new Vector3(-40f, 0, 0); // 첫 회전값
    private Vector3 lastRot = new Vector3(90f, 0, 0); // 레버 작동 시 도달할 회전값

    public bool canInteract;
    public bool isLeverAction;

    private void Start()
    {
        // 레버의 부모 오브젝트에서 ILeverActionable 인터페이스를 가지고 있는 오브젝트를 가져옴
        leverActionObject = GetComponentsInParent<ILeverActionable>();

        // 부모에 존재 하지 않는다면 자식 오브젝트들에게서 가져옴
        if(leverActionObject.Length == 0)
            leverActionObject = GetComponentsInChildren<ILeverActionable>();

        // 둘 다 발견되지 않았을 때를 위한 예외 처리
        if (leverActionObject.Length == 0)
            Debug.LogError("레버와 상호작용 할 오브젝트를 찾지 못했습니다. 설정을 확인해주세요.");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canInteract && !isLeverAction)
        {
            StartCoroutine(RotateLever(firstRot, lastRot));

            // 레버에 할당된 오브젝트들에게 레버 작동 시 호출 될 함수들 실행
            foreach (var lever in leverActionObject)
                lever.StartLeverAction();

            isLeverAction = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText("E키를 입력하여 레버를 작동 시킬 수 있습니다.");
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText(string.Empty);
            canInteract = false;
        }
    }

    // 레버 회전 코루틴
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

    // 레버 위치 초기화 함수
    public void InitLeverRotation()
    {
        StartCoroutine(RotateLever(lastRot, firstRot));
        isLeverAction = false;
    }

    public ObjectInfo GetObjectInfo() => info;
}
