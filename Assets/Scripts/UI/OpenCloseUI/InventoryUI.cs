using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseOpenCloseUI
{
    [SerializeField] private TextMeshProUGUI selectItemNameText;
    [SerializeField] private TextMeshProUGUI selectItemCountText;
    [SerializeField] private TextMeshProUGUI selectItemDescriptionText;
    [SerializeField] private Image selectItemImage;
    [SerializeField] private Button useButton;

    private List<InventorySlot> slots;
    public ItemInfo item;
    private int index;

    public override void Init()
    {
        slots = GetComponentsInChildren<InventorySlot>(true).ToList();
        SetSlot();
    }

    public void SetDescription(InventorySlot slot)
    {
        item = slot.item;
        selectItemNameText.text = slot.item.ItemName;
        selectItemCountText.text = $"º¸À¯:{slot.count.ToString()}";
        selectItemDescriptionText.text = slot.item.ItemDescription;
        selectItemImage.sprite = slot.itemImage.sprite;
        index = slot.index;
    }

    public void SetSlot()
    {
        int slotIndex = 0;

        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.itemImage.gameObject.SetActive(false);
                slot.countText.text = string.Empty;
            }
            else
            {
                slot.itemImage.gameObject.SetActive(true);
                slot.itemImage.sprite = slot.item.ItemSprite;
                slot.countText.text = slot.count.ToString();
            }

            slot.index = slotIndex;
            slotIndex++;
        }
    }

    public void AddItem(ItemInfo _item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == _item)
            {
                slots[i].count++;
                slots[i].countText.text = slots[i].count.ToString();
                SetDescription(slots[i]);
                return;
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = _item;
                slots[i].count++;
                slots[i].countText.text = slots[i].count.ToString();
                SetDescription(slots[i]);
                break;
            }
        }

        SetSlot();
    }

    public void OnClickUseButton()
    {
        if(item != null && ItemManager.Instance.UseItem(item))
        {
            slots[index].count--;

            if (slots[index].count <= 0)
            {

            }
        }
    }
}
