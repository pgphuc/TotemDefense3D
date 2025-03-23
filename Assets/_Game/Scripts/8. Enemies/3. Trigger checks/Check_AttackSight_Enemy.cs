using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Check_AttackSight_Enemy : MonoBehaviour, ITriggerCheck
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
        _moveComponent.StopMoving();
    }
    #endregion
    
    public Component_Move_Enemy _moveComponent;
    private GameObject _target;

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("MeleeMinion") || other.CompareTag("RangedMinion"))
        //     _moveComponent._minionInRange.Add(other);
        if (IsOwnerInCheck)
            return;
        if (other.CompareTag("MeleeMinion"))
        {
            Check_AttackSight_Minion minionCheck = other.GetComponentInChildren<Check_AttackSight_Minion>();
            if (minionCheck.IsOpponentInCheck)
                return;
            minionCheck.SetOpponentInCheckBool(true);
            HandleEnter(other);
        }
        else if (other.CompareTag("RangedMinion"))
        {
            //TODO: implement when done with RangedMinion
            // Check_AttackRanged_Minion minionCheck = other.GetComponentInChildren<Check_AttackRanged_Minion>();
            // if (minionCheck.IsOpponentInCheck)
            //     return;
            // minionCheck.SetOpponentInCheckBool(true);
            HandleEnter(other);
        }
        else if (other.CompareTag("Village"))
        {
            _moveComponent.SetMoveTarget(_moveComponent.FindDestination());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("MeleeMinion") || other.CompareTag("RangedMinion"))
        //     _moveComponent._minionInRange.Remove(other);
        if (_target != other.gameObject) 
            return;
        if (other.CompareTag("MeleeMinion"))
        {
            Check_AttackSight_Minion minionCheck = other.GetComponentInChildren<Check_AttackSight_Minion>();
            minionCheck.SetOpponentInCheckBool(false);
        }
        else if (other.CompareTag("RangedMinion"))
        {
            //TODO: implement when done with RangedMinion
        }
        HandleExit();
    }
    
    
    
}
