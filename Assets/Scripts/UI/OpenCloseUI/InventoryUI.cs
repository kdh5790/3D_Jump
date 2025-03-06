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

    public override void OpenUI()
    {
        base.OpenUI();
        InitSelectItemInfoUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        InitSelectItemInfoUI();
    }

    public void InitSelectItemInfoUI()
    {
        item = null;
        selectItemNameText.text = string.Empty;
        selectItemCountText.text = string.Empty;
        selectItemDescriptionText.text = string.Empty;
        selectItemImage.sprite = null;
        selectItemImage.color = new Color(1, 1, 1, 0);
    }

    public void SetSelectItemInfoUI(InventorySlot slot)
    {
        item = slot.item;
        selectItemNameText.text = slot.item.ItemName;
        selectItemCountText.text = $"보유:{slot.count.ToString()}";
        selectItemDescriptionText.text = slot.item.ItemDescription;
        selectItemImage.color = Color.white;
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
                item = _item;
                slots[i].count++;
                slots[i].countText.text = slots[i].count.ToString();
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
            slots[index].countText.text = slots[index].count.ToString();
            selectItemCountText.text = $"보유:{slots[index].count.ToString()}";

            if (slots[index].count <= 0)
            {
                slots[index].InitSlot();
                InitSelectItemInfoUI();
            }
        }
    }
}
