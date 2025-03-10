using UnityEngine;

[CreateAssetMenu(fileName = "Costume Info", menuName = "Costume Info")]
public class CostumeInfo : ScriptableObject
{
    public string CostumeName;
    public int Health;
    public float Stamina;
    public float Speed;
    public float JumpPower;
}
