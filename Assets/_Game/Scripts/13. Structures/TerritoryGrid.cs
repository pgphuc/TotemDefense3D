using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class TerritoryGrid : MonoBehaviour
{
    public TerritoryState state;
    public GridStructure gridStructure;
    public int territoryID;
    public Renderer gridRenderer;
    [HideInInspector] public bool isFullTotem = false;
    
    public Material[] territoryMaterials;
    
    [SerializeField] private GameUnit _barrackPrefab;
    private BarrackBase _barrack;
    
    [SerializeField] private GameUnit _earthTotemPrefab;
    [SerializeField] private GameUnit _fireTotemPrefab;
    [SerializeField] private GameUnit _iceTotemPrefab;
    [SerializeField] private GameUnit _windTotemPrefab;
    [SerializeField] private GameUnit _lightningTotemPrefab;
    private TotemBase _totem;

    public void UnlockGrid()
    {
        state = TerritoryState.Unlocked;
        gridRenderer.material = territoryMaterials[0];
    }

    public void BuildBarrack()
    {
        _barrack = SimplePool.Spawn<BarrackBase>(_barrackPrefab.poolType, transform.position, Quaternion.identity);
        MapManager.Instance.BarrackNotFullList.Add(_barrack);
        _barrack.OnInit();
    }

    public void BuildEarthTotem()
    {
        _totem = SimplePool.Spawn<TotemBase>(_earthTotemPrefab.poolType, transform.position, Quaternion.identity);
        _totem.OnInit();
    }
    public void BuildFireTotem()
    {
        _totem = SimplePool.Spawn<TotemBase>(_fireTotemPrefab.poolType, transform.position, Quaternion.identity);
        _totem.OnInit();
    }
    public void BuildIceTotem()
    {
        _totem = SimplePool.Spawn<TotemBase>(_iceTotemPrefab.poolType, transform.position, Quaternion.identity);
        _totem.OnInit();
    }
    public void BuildWindTotem()
    {
        _totem = SimplePool.Spawn<TotemBase>(_windTotemPrefab.poolType, transform.position, Quaternion.identity);
        _totem.OnInit();
    }
    public void BuildLightningTotem()
    {
        _totem = SimplePool.Spawn<TotemBase>(_lightningTotemPrefab.poolType, transform.position, Quaternion.identity);
        _totem.OnInit();
    }
}
