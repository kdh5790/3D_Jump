using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public StatusUI statusUI;
    public DescriptionUI descriptionUI;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        inventoryUI.Init();
    }
}
