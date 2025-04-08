using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasVictory : UICanvas
{
    public void MainMenuButton()
    {
        CloseDirectly();
        GameManager.Instance.BackToMainMenu();
    }
    
    public void ReplayButton()
    {
        CloseDirectly();
        GameManager.Instance.Replay();
    }

    public void NextButton()
    {
        CloseDirectly();
        //TODO: Chuyển màn sau
    }
}