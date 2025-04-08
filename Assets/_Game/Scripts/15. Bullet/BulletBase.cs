using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletBase : GameUnit
{
    protected float _damage;
    protected float _speed;
    protected float _acceleration;
    protected bool _isGrounded;

    [SerializeField] protected List<Vector3> _bulletPath = new List<Vector3>();
    [SerializeField] protected int _currentPathIndex;



    public virtual void Shoot(Vector3 start, Vector3 end)
    {
    }

    protected void StopCurrentPath()
    {
        StopAllCoroutines();
    }


    #region GameUnit functions

    public override void OnInit()
    {
        base.OnInit();
        _currentPathIndex = 0;
        _isGrounded = false;
        _bulletPath.Clear();
    }
    #endregion

    protected virtual void HandleBulletHit(Collider other)
    {
        Component_Health enemy = ComponentCache.GetHealthComponent(other);
        enemy.TakeDamage(_damage);
    }


    
}
