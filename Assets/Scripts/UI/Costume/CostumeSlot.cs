using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostumeSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText; // 코스튬 이름 텍스트

    // 코스튬 스탯 텍스트
    [SerializeField] private TextMeshProUGUI healthText; 
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI jumpPowerText;

    [SerializeField] private Button equipButton;

    public CostumeInfo costumeInfo;

    private void Start()
    {
        equipButton = GetComponentInChildren<Button>(true);
        equipButton.onClick.AddListener(OnClickEquipButton);
    }

    // 코스튬 정보를 기반으로 Text 설정
    public void SetText(CostumeInfo info)
    {
        costumeInfo = info;
        nameText.text = info.CostumeName;
        healthText.text = info.Health.ToString();
        staminaText.text = info.Stamina.ToString();
        speedText.text = info.Speed.ToString();
        jumpPowerText.text = info.JumpPower.ToString();
    }

    public void OnClickEquipButton()
    {
        Player.Instance.costume.ChangeCostume(costumeInfo);
    }
}
