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
            // È¹µæ
            Debug.Log($"{itemInfo.ItemName} È¹µæ");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canObtain = true;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText($"E Å°¸¦ ÀÔ·ÂÇØ {itemInfo.ItemName} À»/¸¦ È¹µæ ÇÒ ¼ö ÀÖ½À´Ï´Ù.");
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
