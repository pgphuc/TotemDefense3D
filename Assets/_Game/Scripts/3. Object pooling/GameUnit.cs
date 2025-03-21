using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameUnit : MonoBehaviour
{
    [SerializeField] public PoolType poolType;
    public List<ComponentBase> components = new List<ComponentBase>();

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

