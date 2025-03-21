 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_AttackMelee_Enemy: MonoBehaviour, ITriggerCheck
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
    public void HandleExit()
    {
        SetOwnerInCheckBool(false);
        SetOpponentInCheckBool(false);
        _target = null;
        _attackComponent._target = null;
    }

    public void HandleEnter(Collider other)
    {
        SetOwnerInCheckBool(true);
        _target = other.gameObject;
        _attackComponent._target = ComponentCache.GetHealthComponent(other);
    }

    #endregion
    
    public Component_Attack_Enemy _attackComponent;
    private GameObject _target;

    private void OnTriggerEnter(Collider other)
    {
        if (IsOwnerInCheck)
            return;
        if (other.CompareTag("MeleeMinion") || other.CompareTag("RangedMinion"))
        {
            Check_AttackMelee_Minion minionCheck = other.GetComponentInChildren<Check_AttackMelee_Minion>();
            if (minionCheck.IsOpponentInCheck)
                return;
            minionCheck.SetOpponentInCheckBool(true);
            HandleEnter(other);
        }
        else if (other.CompareTag("Village"))
        {
            Debug.Log("Đánh với nhà chính");
            HandleEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_target != other.gameObject)
            return;
        if (other.CompareTag("Minion") || other.CompareTag("RangedMinion"))
        {
            Check_AttackMelee_Minion minionCheck = other.GetComponentInChildren<Check_AttackMelee_Minion>();
            minionCheck.SetOpponentInCheckBool(false);
        }
        HandleExit();
    }

    

   
}
