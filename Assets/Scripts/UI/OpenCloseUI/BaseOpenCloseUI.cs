using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOpenCloseUI
{
    void OpenUI();
    void CloseUI();
}

public abstract class BaseOpenCloseUI : MonoBehaviour, IOpenCloseUI
{
    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
        Player.Instance.controller.canLook = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
