using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : BaseOpenCloseUI
{
    // 선택한 아이템 정보 텍스트, 이미지
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

    // 선택한 아이템에 맞게 UI 수정
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

    // 모든 슬롯들 세팅
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

    // 아이템 획득 함수
    public void AddItem(ItemInfo _item)
    {
        // 빈슬롯, 매개변수와 동일한 아이템이 존재하는지 확인 후 아이템 새로추가 혹은 카운트 증가

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

    // 사용 버튼 클릭 시 실행
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
