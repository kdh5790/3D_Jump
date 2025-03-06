using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour, IInteractive
{
    public ItemInfo itemInfo;
    public ObjectInfo objectInfo;
    private bool canObtain;

    public ObjectInfo GetObjectInfo()
    {
        return objectInfo;
    }

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
