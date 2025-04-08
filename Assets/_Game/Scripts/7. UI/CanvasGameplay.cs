using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameplay : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Button startButton;
    
    #region territory info
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button territoryButton;
    [SerializeField] private Button barrackButton;
    [SerializeField] private Button[] totemButtons;
    
    private TerritoryGrid selectedTerritoryGrid;
    private Renderer componentRenderer;

    private bool isShoping;
    #endregion
    
    private GameUnit village;
    
    private void OnEnable()
    {
        PlayerInteraction.OnSelectTerritoryGrid += ShowTerritoryPanel;
        PlayerInteraction.OnDeselectTerritoryGrid += HideTerritoryPanel;
        PlayerInteraction.OnDeselectTerritoryGrid += ResetTerritoryColour;
        PlayerInteraction.OnGoldAmountChanged += UpdateCoin;

    }

    public override void Setup()
    {
        base.Setup();
        selectedTerritoryGrid = null;
        
        VillageBase.Instance.OnHealthChanged += HandlehealthChanged;
        UpdateHealth(VillageBase.Instance._healthComponent.MaxHealth);
        
        PlayerInteraction.Instance.OnInit();
        UpdateCoin(PlayerInteraction.GoldAmount);
        
        startButton.gameObject.SetActive(true);
        
        isShoping = false;
    }

    
    #region Event functions (Health + coin)

    private void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void HandlehealthChanged(GameUnit unit)
    {
        UpdateHealth(VillageBase.Instance._healthComponent.CurrentHealth);
    }
    private void UpdateHealth(float health)
    {
        healthText.text = health.ToString();
    }
    #endregion
    
    
    #region player controller
    private void ShowTerritoryPanel(TerritoryGrid grid)//làm sau
    {
        if (selectedTerritoryGrid == null)
        {
            isShoping = true;
            selectedTerritoryGrid = grid;
            CheckTerritoryGrid(grid);
        }
    }

    private void CheckTerritoryGrid(TerritoryGrid grid)
    {
        switch (grid.gridStructure)
        {
            case GridStructure.Empty:
            {
                CheckTerritory(grid);
                break;
            }
            case GridStructure.Barrack:
            {
                //TODO: Hiển thị buff của territory
                //TODO: Hiển thị cửa sổ thông tin Barrack (LÀM SAU)
                /*
                 * 1. số lượng minionCount/minionThreshold
                 * 2. Thông tin barrack
                 * 3. Nút bán barrack
                 */
                ChangeColor(grid);
                break;
            }
            case GridStructure.Totem:
            {
                //TODO: Hiển thị buff của territory
                #region LÀM SAU

                // if (territoryGrid.isFullTotem) 
                // {
                //     //TODO: Vô hiệu hóa tất cả các nút MUA
                // }
                // else
                // {
                //     //TODO: Chỉ cho phép bấm button mua Totem
                // }

                #endregion
                //TODO: Hiển thị cửa sổ thông tin Totem (LÀM SAU)
                /*
                 * 1. Thông tin totem
                 * 2. Nút bán totem
                 */
                ChangeColor(grid);
                break;
            }
        }
    }

    private void CheckTerritory(TerritoryGrid grid)
    {
        switch (grid.state)
        {
            case TerritoryState.Locked:
            {
                //TODO: Hiển thị buff của territory
                ActiveButton(territoryButton);
                foreach (TerritoryGrid territoryGrid in MapManager.gridDictionary[grid.territoryID])
                {
                    ChangeColor(territoryGrid);
                }
                break;
            }
            default:
            {
                //TODO: Hiển thị buff của territory
                ActiveButton(barrackButton);
                foreach (Button button in totemButtons)
                {
                    ActiveButton(button);
                }
                ChangeColor(grid);
                break;
            }
        }
    }
    private void ChangeColor(TerritoryGrid grid)
    {
        componentRenderer = ComponentCache.GetGridRenderer(grid); 
        componentRenderer.material.color *= 1.5f;
    }

    private void ChangeToOriginalColor(TerritoryGrid grid)
    {
        componentRenderer = ComponentCache.GetGridRenderer(grid);
        componentRenderer.material.color /= 1.5f; // Trả lại màu gốc
    }

    private void HideTerritoryPanel(TerritoryGrid grid)
    {
        if (!isShoping)
            return;
        selectedTerritoryGrid = null;
        DeactiveAllButton();
        isShoping = false;
    }
    private void ResetTerritoryColour(TerritoryGrid grid)
    {
        if (grid.state == TerritoryState.Locked)
        {
            foreach (TerritoryGrid territoryGrid in MapManager.gridDictionary[grid.territoryID])
            {
                ChangeToOriginalColor(territoryGrid);
            }
        }
        else
        {
            ChangeToOriginalColor(grid);
        }
    }

    private void DeactiveAllButton()
    {
        territoryButton.interactable = false;
        barrackButton.interactable = false;
        foreach (Button button in totemButtons)
        {
            button.interactable = false;;
        }
    }

    private void ActiveButton(Button button)
    {
        button.interactable = true;
    }

    
    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
        GameManager.Instance.PauseGame();
    }

    public void StartWaveButton()
    {
        GameManager.Instance.StartPlaying();
        startButton.gameObject.SetActive(false);
    }
    #endregion

    
    #region Shop button
    private void HandlePanelAfterBuying()
    {
        PlayerInteraction.Instance.checkInfoGrid = null;
        HideTerritoryPanel(selectedTerritoryGrid);
    }

    public void BuyTerritoryButton()
    {
        if (!selectedTerritoryGrid)
            return;

        if (PlayerInteraction.GoldAmount >= 20)
        {
            PlayerInteraction.GoldAmount -= 20;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);
            
            foreach (TerritoryGrid grid in MapManager.gridDictionary[selectedTerritoryGrid.territoryID])
            {
                grid.UnlockGrid();
            }
        }

        HandlePanelAfterBuying();
    }

    public void BuyBarrackButton()
    {
        if (PlayerInteraction.GoldAmount >= 10)
        {
            PlayerInteraction.GoldAmount -= 10;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);
            
            selectedTerritoryGrid.BuildBarrack();
            selectedTerritoryGrid.gridStructure = GridStructure.Barrack;
        }
        
        HandlePanelAfterBuying();
    }

    public void EarthTotemButton()//check lại
    {
        if (PlayerInteraction.GoldAmount >= 10)
        {
            PlayerInteraction.GoldAmount -= 10;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);
            
            selectedTerritoryGrid.BuildEarthTotem();
            selectedTerritoryGrid.gridStructure = GridStructure.Totem;
        }

        HandlePanelAfterBuying();
    }
    public void FireTotemButton()//check lại
    {
        if (PlayerInteraction.GoldAmount >= 10)
        {
            PlayerInteraction.GoldAmount -= 10;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);
            
            selectedTerritoryGrid.BuildFireTotem();
            selectedTerritoryGrid.gridStructure = GridStructure.Totem;
        }
        
        HandlePanelAfterBuying();
    }
    public void IceTotemButton()//check lại
    {
        if (PlayerInteraction.GoldAmount >= 10)
        {
            PlayerInteraction.GoldAmount -= 10;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);
            
            selectedTerritoryGrid.BuildIceTotem();
            selectedTerritoryGrid.gridStructure = GridStructure.Totem;
        }

        HandlePanelAfterBuying();
    }
    public void WindTotemButton()//check lại
    {
        if (PlayerInteraction.GoldAmount >= 10)
        {
            PlayerInteraction.GoldAmount -= 10;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);

            selectedTerritoryGrid.BuildWindTotem();
            selectedTerritoryGrid.gridStructure = GridStructure.Totem;
        }

        HandlePanelAfterBuying();
    }
    public void LightningTotemButton()//check lại
    {
        if (PlayerInteraction.GoldAmount >= 10)
        {
            PlayerInteraction.GoldAmount -= 10;
            UpdateCoin(PlayerInteraction.GoldAmount);
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.buildTotem);

            selectedTerritoryGrid.BuildLightningTotem();
            selectedTerritoryGrid.gridStructure = GridStructure.Totem;
        }

        HandlePanelAfterBuying();
    }

    #endregion
   
    
}
