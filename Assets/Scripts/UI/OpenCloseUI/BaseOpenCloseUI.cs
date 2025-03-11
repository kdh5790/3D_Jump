using UnityEngine;

// 팝업 UI들에게 추가 할 인터페이스
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

        // UI가 닫힐 때 행동 제한 해제, 커서 숨김
        Player.Instance.controller.canLook = true;
        Player.Instance.controller.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public virtual void OpenUI()
    {
        gameObject.SetActive(true);

        // UI가 열려있을 때 플레이어 행동 제한, 커서 보이도록 설정
        Player.Instance.controller.canLook = false;
        Player.Instance.controller.canMove = false;
        Player.Instance.controller.Stop();
        Cursor.lockState = CursorLockMode.None;
    }
}
