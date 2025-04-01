using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Fire : BulletBase
{
    private float _burnDuration = 3f;
    private float _damagePerSecond = 1f;
    
    public override void Shoot(Vector3 start, Vector3 end)
    {
        _bulletPath = BulletPathCalculator.ParabolPath(start, end);
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
        EffectManager.AddEffect(new Effect_Burn(other, _burnDuration, _damagePerSecond));
        Invoke(nameof(OnDespawn), 1f);
    }
    
}
