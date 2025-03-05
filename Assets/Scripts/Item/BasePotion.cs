using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePotion : MonoBehaviour, IInteractive
{
    public ItemInfo potionInfo;
    public ObjectInfo objectInfo;

    public ObjectInfo GetObjectInfo()
    {
        return objectInfo;
    }

    public abstract void UsePotion();
}
