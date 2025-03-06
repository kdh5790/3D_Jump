using System.Collections;
using System.Collections.Generic;
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
        }

        return false;
    }
}
