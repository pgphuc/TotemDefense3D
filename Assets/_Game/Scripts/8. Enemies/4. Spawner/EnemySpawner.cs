using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameUnit _prefab;
    private EnemyMeleeBase _enemy;
    float spawnTimer = 5f;
    int index = 0;
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && index < 5)
        {
            index++;
            spawnTimer = 5f;
            _enemy = SimplePool.Spawn<EnemyMeleeBase>(_prefab.poolType, transform.position, Quaternion.identity);
            // _enemy = Instantiate(_prefab, transform.position, Quaternion.identity);
            // _enemy.transform.SetParent(transform, true);
            _enemy.OnInit();
        }
    }
}
