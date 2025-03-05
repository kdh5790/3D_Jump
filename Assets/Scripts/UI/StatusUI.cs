using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private Image hpImage;
    [SerializeField] private Image staminaImage;

    private PlayerStats playerStat;

    private void Start()
    {
        playerStat = Player.Instance.stats;

        playerStat.healthUIUpdateAction += UpdateHealthUI;
        playerStat.staminaUIUpdateAction += UpdateStaminaUI;

        hpImage.fillAmount = 1;
        staminaImage.fillAmount = 1;
    }

    private void UpdateHealthUI(float amount)
    {
        hpImage.fillAmount = amount;
    }

    private void UpdateStaminaUI(float amount)
    {
        staminaImage.fillAmount = amount;
    }

}
