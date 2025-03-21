using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    EnemyMelee,
    EnemyRanged,
    MinionMelee,
    MinionRanged,
    Totem,
    EnemyBullet,
    MinionBullet,
    TotemBullet,
    Barrack
}
public enum MinionType
{
    Village,
    Barrack,
}
public enum MovingType
{
    Reinforcing,
    Defending,
    AfterMatch,
    ReachedBarrack,
    ReachedVillage,
}
public enum TerritoryState
{
    Unlocked = 0,
    BarrackBuilt = 1,
    Locked = 2,
    BarrackFull = 4,
}
public enum GridStructure
{
    Empty = 0,
    Barrack = 1,
    Totem = 2,
}

public enum MinionSpawnType
{
    
}

public enum EnemySpawnType
{
    
}

