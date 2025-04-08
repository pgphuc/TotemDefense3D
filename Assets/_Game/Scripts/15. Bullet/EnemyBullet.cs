using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletBase
{
    [SerializeField] private Transform poolTransform;
    #region GameUnit implementation

    public override void OnInit()
    {
        base.OnInit();
        _damage = 1f;
        _speed = 9f;
        _acceleration = 1.1f;
    }

    public override void OnDespawn()
    {
        transform.SetParent(poolTransform);
        base.OnDespawn();
    }

    #endregion
    
    public override void Shoot(Vector3 start, Vector3 end)
    {
        _bulletPath = BulletPathCalculator.ParabolPath(start, end);
        StartCoroutine(MoveAlongPath(_bulletPath));
    }
    
    private IEnumerator MoveAlongPath(List<Vector3> bulletPath)
    {
        transform.SetParent(null);
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
        Invoke(nameof(OnDespawn), 1f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isGrounded)
            return;
        if (other.CompareTag("MeleeEnemy") || other.CompareTag("RangedEnemy"))
        {
            HandleBulletHit(other);
        }
        else if (other.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
}
