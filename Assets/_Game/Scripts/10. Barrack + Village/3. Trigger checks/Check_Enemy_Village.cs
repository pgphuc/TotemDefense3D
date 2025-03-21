using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Enemy_Village : MonoBehaviour
{
    public VillageBase _owner;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MeleeEnemy") && !other.CompareTag("RangedEnemy"))
            return;
        HandleEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("MeleeEnemy") && !other.CompareTag("RangedEnemy"))
            return;
        HandleExit(other);
    }

    public void HandleEnter(Collider other)
    {
        if (_owner.defenseCount > _owner.enemyInRange.Count)
        {
            _owner.enemyInRange.Add(other);
        }
    }

    public void HandleExit(Collider other)
    {
        if (_owner.enemyInRange.Contains(other))
        {
            _owner.enemyInRange.Remove(other);
        }
    }
}
