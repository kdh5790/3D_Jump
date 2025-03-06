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
    public string ItemName; // ������ �̸�
    public string ItemDescription; // ������ ����
    public PotionType PotionType; // ���� Ÿ��
    public Sprite ItemSprite; // ������ ��������Ʈ
    public int Amount; // ȸ����
    public float DurationTime; // ȿ���� �ִٸ� ���ӽð�
}
