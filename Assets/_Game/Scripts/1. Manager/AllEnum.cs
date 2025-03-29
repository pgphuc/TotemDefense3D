using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    EnemyMelee,
    MinionMelee,
    Totem_Earth, Totem_Fire, Totem_Lightning, Totem_Wind, Totem_Ice,
    Totem_Bullet_Earth, Totem_Bullet_Fire, Totem_Bullet_Lightning, Totem_Bullet_Wind, Totem_Bullet_Ice,
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
    ReachedBarrack,
    ReachedVillage,
}
public enum TerritoryState
{
    Unlocked = 0,
    Locked = 2,
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

