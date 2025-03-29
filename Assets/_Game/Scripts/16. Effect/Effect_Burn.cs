using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Burn : EffectBase
{
    public Effect_Burn(Collider target, float duration, float damamePerSecond) : base(target, duration)
    {
        _damamePerSecond = damamePerSecond;
    }
    private float _damamePerSecond;

    private IEnumerator ApplyBurnEffect()
    {
        Component_Health target = ComponentCache.GetHealthComponent(_target);
        if (target == null)
            yield break;//Dừng coroutine ngay lập tức
        while (UpdateEffect())
        {
            target.TakeDamage(_damamePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
    
    
    public override void ApplyEffect()
    {
       CoroutineManager.StartRoutine(ApplyBurnEffect());
    }

    public override void RemoveEffect()
    {
        //TODO: VFX,...
    }
}
