using System.Collections;
using System.Collections.Generic;
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

    public void SetInteractionDescriptionText(string description)
    {
        interactionDescriptionText.text = description;
    }
}
