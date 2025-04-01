using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    
   
    
    //Death + Health event (all unit)
    public event Action<GameUnit> OnDeath;
    public event Action<GameUnit> OnHealthChanged;

    public void InvokeHealthChanged()
    {
        OnHealthChanged?.Invoke(this);
    }
    /*
     * end
     */
    
    
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
    
    
    private void HandlePausedGame()
    {
        foreach (ComponentBase comp in components)
        {
            switch (comp)
            {
                case Component_Move_Enemy enemy:
                    enemy._agent.isStopped = true;
                    return;
                case Component_Move_Minion minion:
                    minion._agent.isStopped = true;
                    return;
            }
        }
    }
    private void HandleResumeGame()
    {
        foreach (ComponentBase comp in components)
        {
            switch (comp)
            {
                case Component_Move_Enemy enemy:
                    enemy._agent.isStopped = false;
                    return;
                case Component_Move_Minion minion:
                    minion._agent.isStopped = false;
                    return;
            }
        }
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
        GameManager.OnPausedGame += HandlePausedGame;
        GameManager.OnResumeGame += HandleResumeGame;
    }

    protected virtual void InitAllComponents()
    {
        
    }
    
    public virtual void OnDespawn()
    {
        OnDeath?.Invoke(this);
        GameManager.OnPausedGame -= HandlePausedGame;
        GameManager.OnResumeGame -= HandleResumeGame;
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

