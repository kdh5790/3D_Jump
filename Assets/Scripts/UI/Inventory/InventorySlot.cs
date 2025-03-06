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
    public int index;
    public int count;
    public TextMeshProUGUI countText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            UIManager.Instance.inventoryUI.SetSelectItemInfoUI(this);
        }
        else
        {
            Debug.Log("아이템이 존재하지 않습니다.");
        }
    }

    public void InitSlot()
    {
        item = null;
        itemImage.gameObject.SetActive(false);
        count = 0;
        countText.text = string.Empty;
    }
}
