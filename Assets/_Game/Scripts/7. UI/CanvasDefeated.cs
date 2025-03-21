using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasDefeated : UICanvas
{
    [SerializeField] TextMeshProUGUI ScoreText;

    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
    }
    
    public void MainMenuButton()
    {
        CloseDirectly();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
    public void ReplayButton()
    {
        //TODO: chơi lại màn
    }
}
