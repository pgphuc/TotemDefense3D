using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable Unity.PerformanceAnalysis
public static class ComponentCache
{
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
    //Component_Attack
    private static Dictionary<Collider, Component_Attack_Enemy> EnemyAttackCache = new Dictionary<Collider, Component_Attack_Enemy>();
    public static Component_Attack_Enemy GetEnemyAttackComponent(Collider collider)
    {
        if (!EnemyAttackCache.ContainsKey(collider))
        {
            GameUnit _unit = GetGameUnit(collider);
            EnemyAttackCache[collider] = _unit.components.Find
                    (_target => _target is Component_Attack_Enemy)
                as Component_Attack_Enemy;
        }
        return EnemyAttackCache[collider];
    }
    private static Dictionary<Collider, Component_Attack_Minion> MinionAttackCache = new Dictionary<Collider, Component_Attack_Minion>();
    public static Component_Attack_Minion GetMinionAttackComponent(Collider collider)
    {
        if (!MinionAttackCache.ContainsKey(collider))
        {
            GameUnit _unit = GetGameUnit(collider);
            MinionAttackCache[collider] = _unit.components.Find
                    (_target => _target is Component_Attack_Minion)
                as Component_Attack_Minion;
        }
        return MinionAttackCache[collider];
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
    private static Dictionary<Collider, Component_Move_Minion> MinionMoveCache = new Dictionary<Collider, Component_Move_Minion>();
    public static Component_Move_Minion GetMinionMoveComponent(Collider collider)
    {
        if (!MinionMoveCache.ContainsKey(collider))
        {
            GameUnit _unit = GetGameUnit(collider);
            MinionMoveCache[collider] = _unit.components.Find
                    (_target => _target is Component_Move_Minion)
                as Component_Move_Minion;
        }
        return MinionMoveCache[collider];
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
