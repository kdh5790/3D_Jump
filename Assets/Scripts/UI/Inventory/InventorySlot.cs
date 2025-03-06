using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemInfo item;
    public Image itemImage;
    public int count;
    public TextMeshProUGUI countText;
    public IUsable usableItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            UIManager.Instance.inventoryUI.SetDescription(this);
        }
        else
        {
            Debug.Log("�������� �������� �ʽ��ϴ�.");
        }
    }
}
