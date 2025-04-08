using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBullet : BulletBase
{
    public Component_Attack_Totem _totemAttackComponent;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_isGrounded)
            return;
        if (other.CompareTag("MeleeEnemy") || other.CompareTag("BomberEnemy"))
        {
            HandleBulletHit(other);
        }
        else if (other.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
    
    #region GameUnit implementation

    public override void OnInit()
    {
        base.OnInit();
        _damage = _totemAttackComponent._damage;
        _speed = _totemAttackComponent._bulletSpeed;
        _acceleration = _totemAttackComponent._bulletAcceleration;

    }

    #endregion
    
}
