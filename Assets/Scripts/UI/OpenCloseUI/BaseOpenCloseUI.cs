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
    public abstract void Init();

    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
        Player.Instance.controller.canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
        Player.Instance.controller.canLook = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
