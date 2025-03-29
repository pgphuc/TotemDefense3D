using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : IEffect
{
    public Collider _target;
    protected float _duration;
    protected float _elapsedTime;

    public EffectBase(Collider target, float duration)
    {   
        _target = target;
        _duration = duration;
        _elapsedTime = 0;
    }
    public virtual void ApplyEffect()
    {
        
    }

    public virtual void RemoveEffect()
    {
        
    }

    public void ResetEffect()
    {
        _elapsedTime = 0;
    }

    public bool UpdateEffect()
    {
        _elapsedTime += Time.deltaTime;
        return _elapsedTime < _duration;
    }
}
