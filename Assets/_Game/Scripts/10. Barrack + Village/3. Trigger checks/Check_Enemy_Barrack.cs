using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check_Enemy_Barrack : MonoBehaviour
{
    public BarrackBase _owner;
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
            _owner.enemyInRange.Add(ComponentCache.GetHealthComponent(other));
        }
    }

    public void HandleExit(Collider other)
    {
        Component_Health target = ComponentCache.GetHealthComponent(other);
        if (_owner.enemyInRange.Contains(target))
        {
            _owner.enemyInRange.Remove(target);
        }
    }
}
