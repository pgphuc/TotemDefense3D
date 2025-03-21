using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Minion_Barrack : MonoBehaviour
{
    public BarrackBase _owner;
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("MeleeMinion") && !collider.CompareTag("RangedMinion"))
            return;
        MinionBase minion = ComponentCache.GetMinion(collider);
        if (!_owner.surroundBarrackPoints.Contains(minion._moveComponent._target)
            && minion._moveComponent._target != _owner.transform.position)
            return;
        switch (minion.minionType)
        {
            case MinionType.Village:
                _owner._minionCapacity--;
                break;
            case MinionType.Barrack:
                minion.movingType = MovingType.ReachedBarrack;
                break;
        }
        minion.OnDespawn();
    }   
    
    
}
