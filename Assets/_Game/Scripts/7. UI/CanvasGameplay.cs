using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGameplay : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI healthText;

    
    #region territory info
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button territoryButton;
    [SerializeField] private Button barrackButton;
    [SerializeField] private Button[] totemButtons;
    
    private TerritoryGrid selectedTerritoryGrid;
    private Renderer componentRenderer;
    private Color? originalColor;
    #endregion
    private void OnEnable()
    {
        PlayerInteraction.OnSelectTerritoryGrid += ShowTerritoryPanel;
        PlayerInteraction.OnDeselectTerritoryGrid += HideTerritoryPanel;
        PlayerInteraction.OnDeselectTerritoryGrid += ResetTerritoryColour;
    }

    public override void Setup()
    {
        base.Setup();
        UpdateCoin(0);
        UpdateHealth(100);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close(float time)
    {
        base.Close(time);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
    }

    private void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    private void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }

    private void ShowTerritoryPanel(TerritoryGrid territoryGrid)//làm sau
    {
        if (selectedTerritoryGrid == null)
        {
            infoPanel.SetActive(true);
            selectedTerritoryGrid = territoryGrid;
            CheckTerritoryGrid(selectedTerritoryGrid);
        }
    }

    private void CheckTerritoryGrid(TerritoryGrid territoryGrid)
    {
        switch (territoryGrid.gridStructure)
        {
            case GridStructure.Empty:
            {
                CheckTerritory(territoryGrid);
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
                ChangeColorOnSelect(territoryGrid);
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
                ChangeColorOnSelect(territoryGrid);
                break;
            }
        }
    }

    private void CheckTerritory(TerritoryGrid territoryGrid)
    {
        switch (territoryGrid.thisTerritory.state)
        {
            case TerritoryState.Locked:
            {
                //TODO: Hiển thị buff của territory
                ActiveButton(territoryButton);
                foreach (TerritoryGrid grid in territoryGrid.thisTerritory.gridsList)
                {
                    ChangeColorOnSelect(grid);
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
                ChangeColorOnSelect(territoryGrid);
                break;
            }
        }
    }
    private void ChangeColorOnSelect(TerritoryGrid grid)
    {
        componentRenderer = ComponentCache.GetGridRenderer(grid);
        originalColor ??= componentRenderer.material.color;//Nếu null thì lấy giá trị gán vô, không thì giữ nguyên
        componentRenderer.material.color = (Color)originalColor * 1.5f;
    }

    private void ChangeToOriginalColor(TerritoryGrid grid)
    {
        if (originalColor == null)
            return;
        componentRenderer = ComponentCache.GetGridRenderer(grid);
        componentRenderer.material.color = (Color)originalColor; // Trả lại màu gốc
    }

    private void HideTerritoryPanel(TerritoryGrid territoryGrid)
    {
        if (!infoPanel.activeSelf)
            return;
        selectedTerritoryGrid = null;
        DeactiveAllButton();
        infoPanel.SetActive(false);
    }
    private void ResetTerritoryColour(TerritoryGrid territoryGrid)
    {
        if (territoryGrid.thisTerritory.state == TerritoryState.Locked)
        {
            foreach (TerritoryGrid grid in territoryGrid.thisTerritory.gridsList)
            {
                ChangeToOriginalColor(grid);
            }
        }
        else
        {
            ChangeToOriginalColor(territoryGrid);
        }
        originalColor = null;
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
    }

    public void BuyTerritoryButton()
    {
        if (!selectedTerritoryGrid)
            return;
        
        selectedTerritoryGrid.thisTerritory.ChangeState(TerritoryState.Unlocked);
        PlayerInteraction.Instance.checkInfoGrid = null;
        HideTerritoryPanel(selectedTerritoryGrid);
        
        originalColor = null;
    }

    public void BuyBarrackButton()
    {
        selectedTerritoryGrid.BuildBarrack();
        selectedTerritoryGrid.gridStructure = GridStructure.Barrack;
        
        selectedTerritoryGrid.thisTerritory.ChangeState(TerritoryState.BarrackBuilt);
        PlayerInteraction.Instance.checkInfoGrid = null;
        HideTerritoryPanel(selectedTerritoryGrid);
        
        originalColor = null;
    }

    public void BuyTotemButton()//Làm sau
    {
        selectedTerritoryGrid.BuildTotem();
        selectedTerritoryGrid.gridStructure = GridStructure.Totem;
        
        PlayerInteraction.Instance.checkInfoGrid = null;
        HideTerritoryPanel(selectedTerritoryGrid);
        
        originalColor = null;
    }
    
    
}
