using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clearOrGameOverText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnClickRestartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OpenUI()
    {
        Player.Instance.controller.Stop();
        Player.Instance.controller.canMove = false;
        Player.Instance.controller.canLook = false;
        Cursor.lockState = CursorLockMode.None;

        gameObject.SetActive(true);
    }

    public void OnClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickExitButton()
    {
        Application.Quit();
    }

    public void SetText(string text)
    {
        clearOrGameOverText.text = text;
    }
}
