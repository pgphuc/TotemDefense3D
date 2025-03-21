using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Minion_Village : MonoBehaviour
{
    public VillageBase _owner;
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("MeleeMinion") && !collider.CompareTag("RangedMinion"))
            return;
        MinionBase minion = ComponentCache.GetMinion(collider);
        if (!MapManager.Instance.surroundBasePoints.ContainsKey(minion._moveComponent._target)
            && minion._moveComponent._target != _owner.transform.position)
            return;
        if (minion.movingType == MovingType.AfterMatch)
            minion.movingType = MovingType.ReachedVillage;
        minion.OnDespawn();
    }   
}
