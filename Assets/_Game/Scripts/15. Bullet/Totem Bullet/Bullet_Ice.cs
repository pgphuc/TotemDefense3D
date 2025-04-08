using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ice : TotemBullet
{
    private float _slowDuration = 2f;
    private float _slowAmount = 0.3f;
    [SerializeField] private GameObject _slowVFX;
    
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
            
            transform.rotation = Quaternion.LookRotation(nextPos - transform.position) * Quaternion.Euler(90, 0, 0);
            
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
        EffectManager.AddEffect(new Effect_Slow(other, _slowDuration, _slowAmount));
        GameObject slowVFX = Instantiate(_slowVFX, other.transform.position, other.transform.rotation, other.transform);
        CoroutineManager.StartRoutine(DestroyVFX(slowVFX));
        Invoke(nameof(OnDespawn), 1f);
    }
    private IEnumerator DestroyVFX(GameObject effectVFX)
    {
        yield return new WaitForSeconds(_slowDuration);
        Destroy(effectVFX);
    }
}
