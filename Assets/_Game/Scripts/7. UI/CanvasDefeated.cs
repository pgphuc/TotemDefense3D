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
        GameManager.Instance.BackToMainMenu();
    }
    public void ReplayButton()
    {
        CloseDirectly();
        GameManager.Instance.Replay();
    }
}
