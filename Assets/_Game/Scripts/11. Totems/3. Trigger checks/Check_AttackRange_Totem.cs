using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Check_AttackRange_Totem : MonoBehaviour
{
    public TotemBase _owner;
    public void HandleEnter(Collider other)
    {
        _owner._attackComponent._targetList.Add(other);
    }

    public void HandleExit(Collider other)
    {
        _owner._attackComponent._targetList.Remove(other);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MeleeEnemy") && !other.CompareTag("RangedEnemy"))
            return;
        HandleEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_owner._attackComponent._targetList.Contains(other))
            return;
        HandleExit(other);
    }

}
