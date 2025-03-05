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
    public string ItemName; // ������ �̸�
    public PotionType PotionType; // ���� Ÿ��
    public int Amount; // ȸ����
    public float DurationTime; // ȿ���� �ִٸ� ���ӽð�
}
