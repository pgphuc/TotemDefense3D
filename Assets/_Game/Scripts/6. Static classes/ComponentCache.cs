using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentCache
{
    //Territory Grid
    private static Dictionary<Collider, TerritoryGrid> TerritoryGridCache = new Dictionary<Collider, TerritoryGrid>();
    // ReSharper disable Unity.PerformanceAnalysis
    public static TerritoryGrid GetTerritoryGrid(Collider collider)
    {
        if (!TerritoryGridCache.ContainsKey(collider))
        {
            TerritoryGridCache[collider] = collider.GetComponent<TerritoryGrid>();
        }
        return TerritoryGridCache[collider];
    }
    
    //Component_Health
    private static Dictionary<Collider, Component_Health> HealthCache = new Dictionary<Collider, Component_Health>();
    // ReSharper disable Unity.PerformanceAnalysis
    public static Component_Health GetHealthComponent(Collider collider)
    {
        if (!HealthCache.ContainsKey(collider))
        {
            GameUnit _unit = collider.GetComponent<GameUnit>();
            HealthCache[collider] = _unit.components.Find
                (_target => _target is Component_Health)
                as Component_Health;
        }
        return HealthCache[collider];
    }
    private static Dictionary<TerritoryGrid, Renderer> GridRendererCache = new Dictionary<TerritoryGrid, Renderer>();

    // ReSharper disable Unity.PerformanceAnalysis
    public static Renderer GetGridRenderer(TerritoryGrid grid)
    {
        if (!GridRendererCache.ContainsKey(grid))
        {
            GridRendererCache[grid] = grid.GetComponent<Renderer>();
        }
        return GridRendererCache[grid];
    }
    
    private static Dictionary<Collider, MinionBase> MinionCache = new Dictionary<Collider, MinionBase>();

    // ReSharper disable Unity.PerformanceAnalysis
    public static MinionBase GetMinion(Collider collider)
    {
        if (!MinionCache.ContainsKey(collider))
        {
            MinionCache[collider] = collider.GetComponent<MinionBase>();
        }
        return MinionCache[collider];
    }
    
    
    
}
