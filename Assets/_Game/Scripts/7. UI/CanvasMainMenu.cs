using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    public void PlayButton()
    {
        CloseDirectly();//Đóng UI main menu
        UIManager.Instance.OpenUI<CanvasGameplay>();
    }

    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
    }

    public void QuitButton()
    {
        Debug.Log("Ai cho quit mà quit, quay lại chơi game ngay !!! ");
    }
    
}