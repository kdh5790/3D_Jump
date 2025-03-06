using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    void UseItem();
}

public abstract class BaseItem : MonoBehaviour, IInteractive, IUsable
{
    public ItemInfo itemInfo;
    public ObjectInfo objectInfo;
    private bool canObtain;

    public ObjectInfo GetObjectInfo()
    {
        return objectInfo;
    }

    public abstract void UseItem();

    protected virtual void Update()
    {
        if (!canObtain) return;


        if (Input.GetKeyDown(KeyCode.E))
        {
            // ȹ��
            Debug.Log($"{itemInfo.ItemName} ȹ��");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canObtain = true;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText($"E Ű�� �Է��� {itemInfo.ItemName} ��/�� ȹ�� �� �� �ֽ��ϴ�.");
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
