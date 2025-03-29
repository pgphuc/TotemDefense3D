using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletBase : GameUnit
{
    public Component_Attack_Totem _totemAttackComponent;
    private float _damage;
    [SerializeField] private bool _isGrounded;

    protected float _speed;
    protected float _acceleration;
    

    [SerializeField] protected List<Vector3> _bulletPath = new List<Vector3>();
    [SerializeField] protected int _currentPathIndex;



    public virtual void Shoot(Vector3 start, Vector3 end)
    {
    }

    protected void StopCurrentPath()
    {
        StopAllCoroutines();
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

    protected virtual void HandleBulletHit(Collider other)
    {
        Component_Health enemy = ComponentCache.GetHealthComponent(other);
        enemy.TakeDamage(_damage);
    }


    #region GameUnit implementation

    public override void OnInit()
    {
        _damage = _totemAttackComponent._damage;
        _speed = _totemAttackComponent._bulletSpeed;
        _acceleration = _totemAttackComponent._bulletAcceleration;
        _currentPathIndex = 0;
        _isGrounded = false;
        _bulletPath.Clear();

    }

    #endregion
}
