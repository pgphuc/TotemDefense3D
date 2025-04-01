using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    public void PlayButton()
    {
        CloseDirectly();//Đóng UI main menu
        //TODO: Gọi sang gameManager để start game
        GameManager.Instance.StartGame();
    }

    public void SettingsButton()
    {
        CloseDirectly();
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Ai cho quit mà quit, quay lại chơi game ngay !!! ");
    }
    
}