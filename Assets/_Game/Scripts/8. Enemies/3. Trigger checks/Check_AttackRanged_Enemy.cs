using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_AttackRanged_Enemy : MonoBehaviour, ITriggerCheck
{
    #region ITriggerCheck implementation
    public bool IsOwnerInCheck { get; set; } = false;
    public void SetOwnerInCheckBool(bool isOwnerInCheck)
    {
        IsOwnerInCheck = isOwnerInCheck;
    }

    public bool IsOpponentInCheck { get; set; } = false;
    public void SetOpponentInCheckBool(bool isOpponentInCheck)
    {
        IsOpponentInCheck = isOpponentInCheck;
    }

    public void HandleEnter(Collider other)
    {
        SetOwnerInCheckBool(true);
        _target = other.gameObject;
        _attackComponent._attackTarget = ComponentCache.GetHealthComponent(other);
    }
    public void HandleExit()
    {
        SetOwnerInCheckBool(false);
        SetOpponentInCheckBool(false);
        _target = null;
        _attackComponent._attackTarget = null;
    }
    #endregion
    
    public Component_Attack_Enemy _attackComponent;
    private GameObject _target;
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsOwnerInCheck)
            return;
        if (other.CompareTag("MeleeMinion"))
        {
            HandleEnter(other);
        }
        else if (other.CompareTag("RangedMinion"))
        {
            HandleEnter(other);
        }
        else if (other.CompareTag("Village"))
        {
            HandleEnter(other);
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        if (_target != other.gameObject)
            return;
        HandleExit();
    }
}
