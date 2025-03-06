using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseOpenCloseUI
{
    [SerializeField] private TextMeshProUGUI selectItemNameText;
    [SerializeField] private TextMeshProUGUI selectItemCountText;
    [SerializeField] private TextMeshProUGUI selectItemDescriptionText;
    [SerializeField] private Image selectItemImage;

    [SerializeField] private InventorySlot[] slots;

    private void Start()
    {
        slots = GetComponentsInChildren<InventorySlot>(true);
        SetSlot();
    }

    public IUsable item;

    public void SetDescription(InventorySlot slot)
    {
        selectItemNameText.text = slot.item.ItemName;
        selectItemCountText.text = $"º¸À¯:{slot.count.ToString()}";
        selectItemDescriptionText.text = slot.item.ItemDescription;
        selectItemImage.sprite = slot.itemImage.sprite;
    }

    public void SetSlot()
    {
        foreach (InventorySlot slot in slots)
        {
            if(slot.item == null)
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
        }
    }

    public void AddItem()
    {

    }
}
