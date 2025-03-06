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

    protected virtual void FixedUpdate()
    {
        if (!canObtain) return;

        if(canObtain && Input.GetKeyDown(KeyCode.E))
        {
            // È¹µæ
            Debug.Log($"{potionInfo.ItemName} È¹µæ");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canObtain = true;
            UIManager.Instance.descriptionUI.SetInteractionDescriptionText($"E Å°¸¦ ÀÔ·ÂÇØ {potionInfo.ItemName} À»/¸¦ È¹µæ ÇÒ ¼ö ÀÖ½À´Ï´Ù.");
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
