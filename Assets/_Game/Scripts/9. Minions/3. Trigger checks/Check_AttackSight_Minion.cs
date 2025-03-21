using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_AttackSight_Minion : MonoBehaviour, ITriggerCheck
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
        _moveComponent._dualingTarget = null;
        _moveComponent.StartMoving();
    }

    public void HandleEnter(Collider other)
    {
        SetOwnerInCheckBool(true);
        _target = other.gameObject;
        _moveComponent._dualingTarget = ComponentCache.GetHealthComponent(other);
        _moveComponent.SetMoveTarget(other.GetComponent<Transform>().position); 
    }
    #endregion
    
    public Component_Move_Minion _moveComponent;
    private GameObject _target;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsOwnerInCheck)
            return;
        if (other.CompareTag("MeleeEnemy"))
        {
            Check_AttackSight_Enemy enemyCheck = other.GetComponentInChildren<Check_AttackSight_Enemy>();
            if (enemyCheck.IsOpponentInCheck)
                return;
            enemyCheck.SetOpponentInCheckBool(true);
            HandleEnter(other);
        }
        else if (other.CompareTag("RangedEnemy"))
        {
            Check_AttackRanged_Enemy enemyCheck = other.GetComponentInChildren<Check_AttackRanged_Enemy>();
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
        if (other.CompareTag("MeleeEnemy"))
        {
            Check_AttackSight_Enemy enemyCheck = other.GetComponentInChildren<Check_AttackSight_Enemy>();
            enemyCheck.SetOpponentInCheckBool(false);
        }
        else if (other.CompareTag("RangedEnemy"))
        {
            Check_AttackRanged_Enemy enemyCheck = other.GetComponentInChildren<Check_AttackRanged_Enemy>();
            enemyCheck.SetOpponentInCheckBool(false);
        }
        HandleExit();
    }
    
    
    
    
}
