using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : Singleton<PlayerInteraction>
{
    public static event Action<TerritoryGrid> OnSelectTerritoryGrid;
    public static event Action<TerritoryGrid> OnDeselectTerritoryGrid;
    public TerritoryGrid checkInfoGrid;
    
    [SerializeField] private LayerMask TerritoryGridMask;
    
    private TerritoryGrid mouseDownGrid;
    
    private bool isPressing = false; //player đang nhấn giữ
    
    
    void Update()
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
    private bool IsPointerOverUIElement()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
    
}
