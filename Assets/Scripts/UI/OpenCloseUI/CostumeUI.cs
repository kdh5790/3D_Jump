using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeUI : BaseOpenCloseUI
{
    [SerializeField] private Button closeButton;

    private List<CostumeInfo> costumeInfos = new List<CostumeInfo>();
    private CostumeSlot[] costumeSlots;

    public override void Init()
    {
        closeButton.onClick.AddListener(CloseUI);

        costumeSlots = GetComponentsInChildren<CostumeSlot>();
        costumeInfos = Player.Instance.costume.GetCostumeInfo();

        SetSlots();
    }

    // 코스튬 슬롯 UI들 세팅
    private void SetSlots()
    {
        if (costumeSlots == null) return;

        for (int i = 0; i < costumeSlots.Length; i++)
        {
            costumeSlots[i].SetText(costumeInfos[i]);
        }
    }
}
