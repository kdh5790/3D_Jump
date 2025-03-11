using TMPro;
using UnityEngine;

public class DescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI interactionDescriptionText;

    private void Start()
    {
        nameText.text = string.Empty;
        descriptionText.text = string.Empty;
        interactionDescriptionText.text = string.Empty;
    }

    // 우측 하단에 표시 될 오브젝트 정보 UI 텍스트 세팅
    public void SetInfoText(ObjectInfo _info = null)
    {
        if (_info != null)
        {
            nameText.text = _info.Name;
            descriptionText.text = _info.Description;
        }
        else
        {
            nameText.text = string.Empty;
            descriptionText.text = string.Empty;
        }
    }

    // 화면 상단에 표시될 상호작용 설명 UI 텍스트 세팅
    public void SetInteractionDescriptionText(string description)
    {
        interactionDescriptionText.text = description;
    }
}
