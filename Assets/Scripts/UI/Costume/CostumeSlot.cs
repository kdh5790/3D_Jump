using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostumeSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
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
