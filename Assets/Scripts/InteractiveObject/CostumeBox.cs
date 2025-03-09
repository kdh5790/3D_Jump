using UnityEngine;

public class CostumeBox : MonoBehaviour, IInteractive
{
    [SerializeField] private ObjectInfo info;

    public ObjectInfo GetObjectInfo()
    {
        return info;
    }
}
