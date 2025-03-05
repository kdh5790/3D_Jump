using UnityEngine;

public enum PotionType
{
    None,
    Health,
    Stamina
}

[CreateAssetMenu(fileName = "Item Info", menuName = "Item Info")]
public class ItemInfo : ScriptableObject
{
    public string ItemName; // 아이템 이름
    public PotionType PotionType; // 포션 타입
    public int Amount; // 회복량
    public float DurationTime; // 효과가 있다면 지속시간
}
