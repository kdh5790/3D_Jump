using System.Collections;
using UnityEngine;

public class CostumeBox : MonoBehaviour, IInteractive
{
    [SerializeField] private ObjectInfo info;
    [SerializeField] private GameObject chestLid; // 상자 뚜껑 오브젝트

    private bool canActive;
    private Coroutine boxCoroutine;

    public ObjectInfo GetObjectInfo() => info;

    private void Update()
    {
        if (canActive && Input.GetKeyDown(KeyCode.F) && boxCoroutine == null)
        {
            UIManager.Instance.costumeUI.OpenUI();
            boxCoroutine = StartCoroutine(InteractiveBox(true));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canActive = true;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText("F키를 입력하여 옷장을 열 수 있습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canActive = false;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText(string.Empty);
            if (boxCoroutine != null)
                StopCoroutine(boxCoroutine);

            boxCoroutine = StartCoroutine(InteractiveBox(false));

            UIManager.Instance.costumeUI.CloseUI();
        }
    }

    // 박스 활성화 (true : 열림, false : 닫힘)
    private IEnumerator InteractiveBox(bool open)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Quaternion startRotation = chestLid.transform.localRotation;
        Quaternion targetRotation = open ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 0);


        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            chestLid.transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        chestLid.transform.localRotation = targetRotation;
        boxCoroutine = null;
    }
}
