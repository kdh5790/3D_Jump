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
                break;
            case PotionType.RecoveryHealth:
                
                break;
            case PotionType.RecoveryStamina:
                break;
        }

        return false;
    }
}
