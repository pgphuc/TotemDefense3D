using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HordeInfo
{
    public GameUnit unit;
    public int quantity;
    public int spawner;
    public float firstSpawnTime;
    public float totalSpawnTime;
}

[CreateAssetMenu(fileName = "Wave Data", menuName = "Scriptable Object/New Wave Data")]
public class EnemyWaveData : ScriptableObject
{
    public List<HordeInfo> hordesList;
    public float timeUntilNextWave;
}