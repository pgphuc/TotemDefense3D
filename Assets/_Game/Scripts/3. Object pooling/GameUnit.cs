using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameUnit : MonoBehaviour
{
    [SerializeField] public PoolType poolType;
    public List<ComponentBase> components = new List<ComponentBase>();
    
    
    
    #region event implementation
    
    //Barrack && Village event (enemy-related)
    public event Action<GameUnit> EnterAttack;
    public event Action<GameUnit> ExitAttack;

    public void InvokeEnterAttack()
    {
        EnterAttack?.Invoke(this);
    }
    public void InvokeExitAttack()
    {
        ExitAttack?.Invoke(this);
    }
    /*
     * end
     */
    
   
    
    //Death event (all unit)
    public event Action<GameUnit> OnDeath;
    public virtual void SubcribeAllEvents(GameUnit unit)
    {
        unit.OnDeath += OnTargetDeath;
    }

    protected virtual void UnSubcribeAllEvents(GameUnit unit)
    {
        unit.OnDeath -= OnTargetDeath;
    }

    protected virtual void OnTargetDeath(GameUnit unit)
    {
        UnSubcribeAllEvents(unit);
    }
    
    #endregion

    
    
    public virtual void Awake()
    {
        ComponentConstructor();
        StateMachineConstructor();
    }

    protected virtual void StateMachineConstructor()
    {
        
    }

    protected virtual void ComponentConstructor()
    {
        
    }
    public virtual void OnInit()
    {
        InitAllComponents();
    }

    protected virtual void InitAllComponents()
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

