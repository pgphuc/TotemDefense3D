using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_AttackMelee_Minion : MonoBehaviour, ITriggerCheck
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
        _attackComponent._attackTarget = null;
    }

    public void HandleEnter(Collider other)
    {
        SetOwnerInCheckBool(true);
        _target = other.gameObject;
        _attackComponent._attackTarget = ComponentCache.GetHealthComponent(other);
    }
    #endregion
    
    public Component_Attack_Minion _attackComponent;
    private GameObject _target;
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsOwnerInCheck)
            return;
        if (other.CompareTag("MeleeEnemy") || other.CompareTag("RangedEnemy"))
        {
            Check_AttackMelee_Enemy enemyCheck = other.GetComponentInChildren<Check_AttackMelee_Enemy>();
            if (enemyCheck.IsOpponentInCheck)
                return;
            enemyCheck.SetOpponentInCheckBool(true);
            HandleEnter(other);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (_target != other.gameObject)
            return;
        if (other.CompareTag("MeleeEnemy") || other.CompareTag("RangedEnemy"))
        {
            Check_AttackMelee_Enemy enemyCheck = other.GetComponentInChildren<Check_AttackMelee_Enemy>();
            enemyCheck.SetOpponentInCheckBool(false);
        }
        HandleExit();
    }
    
    
    
}
