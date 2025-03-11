using UnityEngine;

public class Item : MonoBehaviour, IInteractive
{
    public ItemInfo itemInfo; // 아이템 정보 (Scriptable Object)
    public ObjectInfo objectInfo; // 오브젝트 정보 (Scriptable Object)
    private bool canObtain; // 획득 가능 여부

    public ObjectInfo GetObjectInfo() => objectInfo;

    protected virtual void Update()
    {
        if (!canObtain) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.inventoryUI.AddItem(itemInfo);
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText(string.Empty);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canObtain = true;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText($"E 키를 입력해 {itemInfo.ItemName} 을/를 획득 할 수 있습니다.");
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canObtain = false;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText(string.Empty);
        }
    }
}
