using UnityEngine;

public enum PotionType
{
    None,
    RecoveryHealth,
    RecoveryStamina,
    InvincibilityPotion,
    SpeedBoostPotion
}

[CreateAssetMenu(fileName = "Item Info", menuName = "Item Info")]
public class ItemInfo : ScriptableObject
{
    public string ItemName; // 아이템 이름
    public string ItemDescription; // 아이템 설명
    public PotionType PotionType; // 포션 타입
    public Sprite ItemSprite; // 아이템 스프라이트
    public int Amount; // 회복량
    public float DurationTime; // 효과가 있다면 지속시간
}
