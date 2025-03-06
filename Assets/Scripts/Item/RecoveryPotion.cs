using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryPotion : BaseItem
{
    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.Instance.inventoryUI.OpenUI();
        }
    }

    public override void UseItem()
    {
       
    }
}
