using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        MainMenu = 0,
        PrePlay = 1,
        Playing = 2,
        GameOver = 3,
    }
    private GameState gameState;
    
    //Paused game
    public static event Action OnPausedGame;
    public static event Action OnResumeGame;

    private bool isDefeated;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
        isDefeated =false;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        ChangeState(GameState.MainMenu);
    }

    private void ChangeState(GameState newState)
    {
        gameState = newState;
        switch (gameState)
        {
            case GameState.MainMenu:
                //TODO: Gọi tới UIManager để mở canvas Main menu
                MainMenuState();
                break; 
            case GameState.PrePlay:
                //TODO: Gen map, và thực hiện pause game
                PrePlayState();
                break; 
            case GameState.Playing:
                //TODO: Tắt pause game
                PlayingState();
                break;
            case GameState.GameOver:
                //TODO: Show màn hình kết thúc game Won/Lost
                Invoke(nameof(GamOverState), 4f);
                break;
        }
    }

    private void Update()
    {
        if (EnemyWaveManager.Instance.IsGameOver() && gameState == GameState.Playing)
        {
            GameOver();
        }
    }

    private void MainMenuState()
    {
        Time.timeScale = 1;
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        
    }

    private void PrePlayState()
    {
        Time.timeScale = 0;
        MapManager.Instance.OnInit();//gen map
        UIManager.Instance.OpenUI<CanvasGameplay>();//Open canva gameplay
    }

    private void PlayingState()
    {
        UIManager.Instance.CloseUIDirectly<CanvasSettings>();
        Time.timeScale = 1;
        VillageBase.OnDefeated += IsDefeated;
        EnemyWaveManager.Instance.OnInit();
    }

    private void GamOverState()
    {
        Time.timeScale = 0;
        UIManager.Instance.CloseAllUI();
        SoundManager.Instance.musicSource.Stop();
        if (isDefeated)
        {
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.defeated);
            UIManager.Instance.OpenUI<CanvasDefeated>();
        }
        else
        {
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.victory);
            UIManager.Instance.OpenUI<CanvasVictory>();
        }
        CancelInvoke(); 
    }

    private void IsDefeated()
    {
        isDefeated = true;
        GameOver();
    }

    private void ResetGame()
    {
        ComponentCache.ResetCache();
        SimplePool.CollectAll();
        UIManager.Instance.CloseAllUI();
        CoroutineManager.StopAllRoutine();
        EffectManager.ResetEffectDictionary();
        MapManager.Instance.OnDestroy();
        isDefeated = false;
    }
    
    
    
    
    
    
    
    
    

    public void Replay()
    {
        ResetGame();
        StartGame();
    }

    public void StartGame()
    {
        ChangeState(GameState.PrePlay);
    }

    public void StartPlaying()
    {
        ChangeState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (gameState == GameState.Playing)
        {
            Time.timeScale = 0;
            OnPausedGame?.Invoke();
        }
    }

    public void ResumeGame()
    {
        if (gameState == GameState.Playing)
        {
            Time.timeScale = 1;
            OnResumeGame?.Invoke();
        }
    }

    public void GameOver()
    {                                                                                     
        ChangeState(GameState.GameOver);
    }

    public void BackToMainMenu()//Back to main menu
    {
        ResetGame();
        ChangeState(GameState.MainMenu);
    }
    

}
