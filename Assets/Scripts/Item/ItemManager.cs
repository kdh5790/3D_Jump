using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public bool UseItem(ItemInfo info)
    {
        switch (info.PotionType)
        {
            case PotionType.None:
                return false;

            case PotionType.RecoveryHealth:
                Player.Instance.stats.HealHealth(info.Amount);
                return true;

            case PotionType.RecoveryStamina:
                Player.Instance.stats.HealStamina(info.Amount);
                return true;

            case PotionType.SpeedBoostPotion:
                Player.Instance.controller.ApplySpeedBoost(info.DurationTime);
                return true;

            case PotionType.InvincibilityPotion:
                Player.Instance.stats.ApplyInvicibility(info.DurationTime);
                return true;
        }

        return false;
    }
}
