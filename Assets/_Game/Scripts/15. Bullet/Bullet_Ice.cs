using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ice : BulletBase
{
    private float _slowDuration = 3f;
    private float _slowAmount = 0.3f;
    
    public override void Shoot(Vector3 start, Vector3 end)
    {
        _bulletPath = BulletPathCalculator.ParabolPath(start, end);
        StartCoroutine(MoveAlongPath());
    }
    
    private IEnumerator MoveAlongPath()
    {
        while (_currentPathIndex < _bulletPath.Count)
        {
            Vector3 nextPos = _bulletPath[_currentPathIndex];
            
            transform.rotation = Quaternion.LookRotation(nextPos - transform.position);
            
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
        MapManager.AddEffect(new Effect_Slow(other, _slowDuration, _slowAmount));
        Invoke(nameof(OnDespawn), 1f);
    }
}
