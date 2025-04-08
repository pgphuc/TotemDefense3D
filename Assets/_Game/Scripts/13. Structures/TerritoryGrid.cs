using UnityEngine;

public class TerritoryGrid : MonoBehaviour
{
    [HideInInspector] public TerritoryState state;
    [HideInInspector] public GridStructure gridStructure;
    [HideInInspector] public bool isGreener;
    [HideInInspector] public int territoryID;
    [HideInInspector] public bool isFullTotem = false;
    [HideInInspector] public Material territoryMaterials;
    
    public Renderer gridRenderer;

    [SerializeField] private GameUnit _barrackPrefab;
    [SerializeField] private GameUnit _earthTotemPrefab;
    [SerializeField] private GameUnit _fireTotemPrefab;
    [SerializeField] private GameUnit _iceTotemPrefab;
    [SerializeField] private GameUnit _windTotemPrefab;
    [SerializeField] private GameUnit _lightningTotemPrefab;
    
    private TotemBase _totem;
    private BarrackBase _barrack;

    public void UnlockGrid()
    {
        state = TerritoryState.Unlocked;
        gridRenderer.material = territoryMaterials;
    }

    public void BuildBarrack()
    {
        _barrack = SimplePool.Spawn<BarrackBase>(_barrackPrefab.poolType, transform.position, Quaternion.identity);
        MapManager.Instance.BarrackList.Add(_barrack);
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
