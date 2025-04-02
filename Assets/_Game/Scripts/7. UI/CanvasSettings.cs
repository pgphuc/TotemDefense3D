using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    [SerializeField] GameObject[] buttons;

    public void SetState(UICanvas canvas)
    {
        foreach (GameObject button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        switch (canvas)
        {
            case CanvasMainMenu:
                buttons[0].gameObject.SetActive(true);
                break;
            case CanvasGameplay:
                buttons[1].gameObject.SetActive(true);
                buttons[2].gameObject.SetActive(true);
                buttons[3].gameObject.SetActive(true);
                break;
        }
    }
    
    public void MainMenuButton()
    {
        CloseDirectly();
        GameManager.Instance.BackToMainMenu();
    }

    public void ClosedButton()
    {
        CloseDirectly();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    public void ContinueButton()
    {
        CloseDirectly();
        GameManager.Instance.ResumeGame();
    }

    public void RestartButton()
    {
        CloseDirectly();
        GameManager.Instance.Replay();
    }
}