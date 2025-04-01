using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : Singleton<PlayerInteraction>
{
    public static Action<int> OnGoldAmountChanged;
    public static int GoldAmount;
    
    
    public static event Action<TerritoryGrid> OnSelectTerritoryGrid;
    public static event Action<TerritoryGrid> OnDeselectTerritoryGrid;
    [HideInInspector] public TerritoryGrid checkInfoGrid;
    
    [SerializeField] private LayerMask TerritoryGridMask;
    
    private TerritoryGrid mouseDownGrid;

    private bool isPressing;//player đang nhấn giữ
    
    public void OnInit()
    {
        GoldAmount = 100;
        isPressing = false;
        EnemySpawner.EnemySpawned += SubcribeEnemyOnDeath;
        
    }
    void Update()
    {
        CheckMouseAction();
    }
    
    
    #region show info functions
    private void CheckMouseAction()
    {
        if (IsPointerOverUIElement())
            return;
        if (Input.GetMouseButtonDown(0) && !isPressing)
        {
            HandleMouseDown();
        }
        if (Input.GetMouseButtonUp(0) && isPressing)
        {
            HandleMouseUp();
        }
    }

    private bool IsPointerOverUIElement()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
    private void HandleMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, TerritoryGridMask))
        {
            mouseDownGrid = ComponentCache.GetTerritoryGrid(hit.collider);
        }
        isPressing = true;
    }

    private void HandleMouseUp()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,TerritoryGridMask))
        {
            if (ComponentCache.GetTerritoryGrid(hit.collider) != mouseDownGrid)
                return;//nếu khi thả chuột ra, vị trí khác với lúc nhấn giữ
            if (checkInfoGrid)
            {//Trường hợp đang xem info của terri 1 mà chọn sang terri 2 thì đóng panel của terri 1
                HideInfoPanel();
            }
            ShowInfoPanel();
        }
        else if (checkInfoGrid)//trường hợp khi click chuột ra ngoài và có panel đang mở
        {
            HideInfoPanel();
        }
        isPressing = false;
        mouseDownGrid = null;
    }

    private void ShowInfoPanel()
    {
        checkInfoGrid = mouseDownGrid;
        OnSelectTerritoryGrid?.Invoke(checkInfoGrid);
    }

    private void HideInfoPanel()
    {
        OnDeselectTerritoryGrid?.Invoke(checkInfoGrid);
        checkInfoGrid = null;
    }
    #endregion
    
    #region Gold functions
    private void SubcribeEnemyOnDeath(GameUnit enemy)
    {
        enemy.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(GameUnit unit)
    {
        if (unit is EnemyBase enemy)
        {
            GoldAmount += enemy.goldAmount;
            OnGoldAmountChanged?.Invoke(GoldAmount);
            unit.OnDeath -= HandleEnemyDeath;
        }
        
    }
    
    #endregion
    
}
