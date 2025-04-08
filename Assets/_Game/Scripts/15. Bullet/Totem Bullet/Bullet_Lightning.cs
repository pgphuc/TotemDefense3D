using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Lightning : TotemBullet
{
    private Vector3 _startPos;
    
    private const int _maxChain = 3;
    private const float _chainRange = 30f;
    
    private int _currentChain;
    
    
    private List<Collider> hitEnemies = new List<Collider>();

    [SerializeField] private GameObject lightningEffectPrefab;

    public override void OnInit()
    {
        base.OnInit();
        _currentChain = 0;
        hitEnemies.Clear();
    }
    public override void Shoot(Vector3 start, Vector3 end)
    {
        _bulletPath = BulletPathCalculator.StraightPath(start, end);
        _startPos = transform.position;
        StartCoroutine(MoveAlongPath(_bulletPath));
    }
    
    private IEnumerator MoveAlongPath(List<Vector3> bulletPath)
    {
        while (_currentPathIndex < bulletPath.Count)
        {
            Vector3 nextPos = bulletPath[_currentPathIndex];
            
            while (Vector3.Distance(transform.position, nextPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, _speed * Time.deltaTime);
                yield return null; // Chờ frame tiếp theo
            }
            _currentPathIndex++;
            _speed *= _acceleration;
        }
        Invoke(nameof(OnDespawn), 1f);
    }

    protected override void HandleBulletHit(Collider other)
    {
        base.HandleBulletHit(other);
        CreateLightningEffect(_startPos, transform.position);
        hitEnemies.Add(other);
        
        if (_currentChain >= _maxChain)
        {
            Invoke(nameof(OnDespawn), 1f);
            return;
        }
        
        StopCurrentPath();
        Collider nextTarget = FindNextEnemy(other);
        
        if (nextTarget != null)
        {
            ShootNextTarget(other.transform.position, nextTarget.transform.position);
        }
        else
        {
            Invoke(nameof(OnDespawn), 1f);
        }

    }

    private void CreateLightningEffect(Vector3 startPos, Vector3 endPos)
    {
        GameObject effect = Instantiate(lightningEffectPrefab, startPos, Quaternion.identity);
        LineRenderer lr = effect.GetComponent<LineRenderer>();
        if (lr != null)
        {
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);
        }
        Destroy(effect, 0.3f);
    }

    private Collider FindNextEnemy(Collider other)
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(other.transform.position, _chainRange, LayerMask.GetMask("Enemy"));
        if (enemiesInRange.Length <= 0)
            return null;
        Collider nextTarget = null;
        float minDistance = float.MaxValue;
        foreach (Collider enemy in enemiesInRange)
        {
            if (hitEnemies.Contains(enemy))
                continue;
            float distance = Vector3.Distance(other.transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nextTarget = enemy;
            }
        }
        return nextTarget;
    }

    private void ShootNextTarget(Vector3 currentTarget, Vector3 nextTarget)
    {
        _currentChain++;
        _speed = _totemAttackComponent._bulletSpeed;
        Shoot(currentTarget, nextTarget);
    }
}
