using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : UICanvas
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    
    private void Start()
    {
        StartMusicVolume();
        StartSfxVolume();
    }

    private void StartMusicVolume()
    {
        float currentVolume = SoundManager.Instance.GetMusicVolume();
        musicSlider.value = currentVolume;
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void StartSfxVolume()
    {
        float currentVolume = SoundManager.Instance.GetSfxVolume();
        sfxSlider.value = currentVolume;
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }
    private void OnMusicVolumeChanged(float value)
    {
        SoundManager.Instance.SetMusicVolume(value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        SoundManager.Instance.SetSfxVolume(value);
    }
    
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