using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable Unity.PerformanceAnalysis
public static class ComponentCache
{
    public static void ResetCache()
    {
        GameUnitCache = new Dictionary<Collider, GameUnit>();
        HealthCache = new Dictionary<Collider, Component_Health>();
        EnemyMoveCache = new Dictionary<Collider, Component_Move_Enemy>();
        TerritoryGridCache =  new Dictionary<Collider, TerritoryGrid>();
        
        GridRendererCache = new Dictionary<TerritoryGrid, Renderer>();
        ColliderCache = new Dictionary<GameUnit, Collider>();
    }
    private static Dictionary<GameUnit, Collider> ColliderCache = new Dictionary<GameUnit, Collider>();
    public static Collider GetCollider(GameUnit gameUnit)
    {
        if (!ColliderCache.ContainsKey(gameUnit))
        {
            ColliderCache[gameUnit] = gameUnit.GetComponent<Collider>();
        }
        return ColliderCache[gameUnit];
    }
    //Unit cache
    private static Dictionary<Collider, GameUnit> GameUnitCache = new Dictionary<Collider, GameUnit>();
    public static GameUnit GetGameUnit(Collider collider)
    {
        if (!GameUnitCache.ContainsKey(collider))
        {
            GameUnitCache[collider] = collider.GetComponent<GameUnit>();
        }
        return GameUnitCache[collider];
    }
    
    private static Dictionary<Collider, MinionBase> MinionCache = new Dictionary<Collider, MinionBase>();
    public static MinionBase GetMinion(Collider collider)
    {
        if (!MinionCache.ContainsKey(collider))
        {
            MinionCache[collider] = collider.GetComponent<MinionBase>();
        }
        return MinionCache[collider];
    }
    
    //Component_Health
    private static Dictionary<Collider, Component_Health> HealthCache = new Dictionary<Collider, Component_Health>();
    public static Component_Health GetHealthComponent(Collider collider)
    {
        if (!HealthCache.ContainsKey(collider))
        {
            GameUnit _unit = GetGameUnit(collider);
            HealthCache[collider] = _unit.components.Find
                (_target => _target is Component_Health)
                as Component_Health;
        }
        return HealthCache[collider];
    }
    //Component_Move
    private static Dictionary<Collider, Component_Move_Enemy> EnemyMoveCache = new Dictionary<Collider, Component_Move_Enemy>();
    public static Component_Move_Enemy GetEnemyMoveComponent(Collider collider)
    {
        if (!EnemyMoveCache.ContainsKey(collider))
        {
            GameUnit _unit = GetGameUnit(collider);
            EnemyMoveCache[collider] = _unit.components.Find
                    (_target => _target is Component_Move_Enemy)
                as Component_Move_Enemy;
        }
        return EnemyMoveCache[collider];
    }
    //Territory
    private static Dictionary<TerritoryGrid, Renderer> GridRendererCache = new Dictionary<TerritoryGrid, Renderer>();
    public static Renderer GetGridRenderer(TerritoryGrid grid)
    {
        if (!GridRendererCache.ContainsKey(grid))
        {
            GridRendererCache[grid] = grid.GetComponent<Renderer>();
        }
        return GridRendererCache[grid];
    }
    private static Dictionary<Collider, TerritoryGrid> TerritoryGridCache = new Dictionary<Collider, TerritoryGrid>();
    public static TerritoryGrid GetTerritoryGrid(Collider collider)
    {
        if (!TerritoryGridCache.ContainsKey(collider))
        {
            TerritoryGridCache[collider] = collider.GetComponent<TerritoryGrid>();
        }
        return TerritoryGridCache[collider];
    }
    
    
    
}
