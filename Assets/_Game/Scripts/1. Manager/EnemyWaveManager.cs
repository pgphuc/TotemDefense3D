using System;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using System.Collections;
using System.Linq;

public class EnemyWaveManager : Singleton<EnemyWaveManager>
{
    [SerializeField] private List<EnemyWaveData> enemyWaveDatas;

    public static Action<EnemyBase> EnemySpawned;
    
    [HideInInspector] public List<Vector3> spawnerPosition = new List<Vector3>();

    [HideInInspector] public List<EnemyBase> enemyOnScene = new List<EnemyBase>();
    
    [HideInInspector] public float gameTimer;
    private float waveTimer;
    private int totalWaves;
    private int wavesCount;

    
    private int totalEnemiesSpawned;
    private int totalEnemiesInWave;
    private bool allEnemyIsSpawned;
    
    public void OnInit()
    {
        StopAllCoroutines();
        totalWaves = enemyWaveDatas.Count;
        wavesCount = 0;
        gameTimer = 0f;
        
        allEnemyIsSpawned = false;
        totalEnemiesInWave = CalculateTotalEnemiesInWave();
        totalEnemiesSpawned = 0;
    }

    private int CalculateTotalEnemiesInWave()
    {
        return enemyWaveDatas.SelectMany(wave => wave.hordesList).Sum(horde => horde.quantity);
    }

    public bool IsGameOver()
    {
        return allEnemyIsSpawned && enemyOnScene.Count == 0;
    }

    private void Update()
    {
        if (totalEnemiesSpawned == totalEnemiesInWave)
            allEnemyIsSpawned = true;
        if (wavesCount == totalWaves)
            return;
        CheckWaveTimer();
    }

    private void CheckWaveTimer()
    {
        waveTimer += Time.deltaTime;
        if (TimeToSpawnEnemy())
        {
            StartNextWave();
        }
    }

    private bool TimeToSpawnEnemy()
    {
        return waveTimer > enemyWaveDatas[wavesCount].timeUntilNextWave;
    }
    private void StartNextWave()
    {
        SetUpWaveSpawner(wavesCount);
        waveTimer = 0f;
        wavesCount++;
        if (wavesCount == totalWaves)//nếu là wave cuối thì mở sound
        {
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.battan);
            SoundManager.Instance.PlayMusic(SoundManager.Instance.bossMusic);
        }
        else
        {
            SoundManager.Instance.PlaySoundOneShot(SoundManager.Instance.startWave);
        }
    }

    private void SetUpWaveSpawner(int wavesIndex)
    {
        int hordesNumber = enemyWaveDatas[wavesIndex].hordesList.Count;
        for (int i = 0; i < hordesNumber; i++)
        {
            HordeInfo horde = enemyWaveDatas[wavesIndex].hordesList[i];
            StartCoroutine(SpawnHordeCustomMode
                (horde.unit, spawnerPosition[horde.spawner], horde.quantity, horde.firstSpawnTime, horde.totalSpawnTime/horde.quantity));
        }
    }

    private IEnumerator SpawnHordeCustomMode(GameUnit unit, Vector3 spawnPosition, int quantity, float firstSpawnTime, float delaySpawnTime)
    {
        yield return new WaitForSeconds(firstSpawnTime);
        
        for (int i = 0; i < quantity; i++)
        {
            SpawnEnemy(unit, spawnPosition);
            yield return new WaitForSeconds(delaySpawnTime);
        }
    }

    private void SpawnEnemy(GameUnit unit, Vector3 spawnPosition)
    {
        EnemyBase enemy = SimplePool.Spawn<EnemyBase>(unit.poolType, spawnPosition, Quaternion.identity);
        enemy.OnInit();
        enemyOnScene.Add(enemy);
        totalEnemiesSpawned++;
        EnemySpawned?.Invoke(enemy);
    }
}