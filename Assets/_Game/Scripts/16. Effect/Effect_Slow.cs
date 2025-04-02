using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Slow : EffectBase
{
    public Effect_Slow(Collider target, float duration, float slowAmount) : base(target, duration)
    {
        _target = ComponentCache.GetEnemyMoveComponent(target);
        _slowAmount = slowAmount;
    }
    
    private Component_Move_Enemy _target;
    private float _slowAmount;
    public override void ApplyEffect()
    {
        _target._agent.speed *= _slowAmount;
    }

    public override void RemoveEffect()
    {
        _target._agent.speed /= _slowAmount;
    }
}
