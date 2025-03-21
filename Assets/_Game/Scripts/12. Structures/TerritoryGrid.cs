using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class TerritoryGrid : MonoBehaviour
{
    [HideInInspector] public GridStructure gridStructure;
    [HideInInspector] public Territory thisTerritory;
    [HideInInspector] public bool isFullTotem = false;
    
    public Material[] territoryMaterials;
    
    [SerializeField] private GameUnit _barrackPrefab;
    private BarrackBase _barrack;
    
    [SerializeField] private GameUnit _totemPrefab;
    private TotemBase _totem;
    
    

    public void BuildBarrack()
    {
        _barrack = SimplePool.Spawn<BarrackBase>(_barrackPrefab.poolType, transform.position, Quaternion.identity);
        _barrack._territory = thisTerritory;
        MapManager.Instance.BarrackNotFullList.Add(_barrack);
        _barrack.OnInit();
    }

    public void BuildTotem()
    {
        _totem = SimplePool.Spawn<TotemBase>(_totemPrefab.poolType, transform.position, Quaternion.identity);
        _totem.OnInit();
    }
}
