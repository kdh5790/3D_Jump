using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemInfo item; // 아이템 정보
    public Image itemImage; // 아이템 이미지
    public int index; // 현재 슬롯의 인덱스
    public int count; // 보유 갯수
    public TextMeshProUGUI countText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            // 슬롯 클릭 시 현재 아이템 정보 UI 변경
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
