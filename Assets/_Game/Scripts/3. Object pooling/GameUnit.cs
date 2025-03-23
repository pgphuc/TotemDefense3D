using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameUnit : MonoBehaviour
{
    [SerializeField] public PoolType poolType;
    public List<ComponentBase> components = new List<ComponentBase>();
    
    
    
    #region event implementation
    
    public event Action<GameUnit> OnDeath;
    public void SubDeathEvent(GameUnit unit)
    {
        unit.OnDeath += OnTargetDeath;
    }

    private void UnSubDeathEvent(GameUnit unit)
    {
        unit.OnDeath -= OnTargetDeath;
    }

    protected virtual void OnTargetDeath(GameUnit unit)
    {
        UnSubDeathEvent(unit);
    }
    
    #endregion

    public virtual void Awake()
    {
        ComponentConstructor();
        StateMachineConstructor();
    }

    public virtual void StateMachineConstructor()
    {
        
    }
    public virtual void ComponentConstructor()
    {
        
    }
    public virtual void OnInit()
    {
        InitAllComponents();
    }
    
    public virtual void InitAllComponents()
    {
        
    }
    
    public virtual void OnDespawn()
    {
        OnDeath?.Invoke(this);
        SimplePool.Despawn(this);
    }

    private IEnumerator SetActiveFalse()
    {
        yield return new WaitForFixedUpdate();
        SimplePool.Despawn(this);
    }
    #region Animation Triggers implementation
    public enum AnimationTriggerType
    {
        Damaged = 0,
        PlayFootstepSound = 2,
        Despawn = 3,
    }
    #endregion
    
}

