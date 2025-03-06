using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePotion : MonoBehaviour, IInteractive
{
    public ItemInfo potionInfo;
    public ObjectInfo objectInfo;
    private bool canObtain;

    public ObjectInfo GetObjectInfo()
    {
        return objectInfo;
    }

    public abstract void UsePotion();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canObtain = true;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText($"E Ű�� �Է��� {potionInfo.ItemName} ��/�� ȹ�� �� �� �ֽ��ϴ�.");
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
