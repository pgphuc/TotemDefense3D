using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    #region Effect Manager
    private static Dictionary<Collider, List<EffectBase>> EffectDictionary = new Dictionary<Collider, List<EffectBase>>();

    public static void ResetEffectDictionary()
    {
        EffectDictionary.Clear();
    }
    public static void AddEffect(EffectBase effect)
    {
        Collider target = effect._target;
        if (!EffectDictionary.ContainsKey(target))
            EffectDictionary[target] = new List<EffectBase>();
        EffectBase existingEffect = FindExistingEffect(EffectDictionary[target], effect);
        if (existingEffect != null)
        {
            existingEffect.ResetEffect();
        }
        else
        {
            EffectDictionary[target].Add(effect);
            effect.ApplyEffect();
        }
        
    }

    private static EffectBase FindExistingEffect(List<EffectBase> effectList, EffectBase searchEffect)
    {
        foreach (EffectBase effect in effectList)
        {
            if (effect == searchEffect)
                return effect;
        }
        return null;
    }

    private static void RemoveEffect(EffectBase effect)
    {
        Collider target = effect._target;
        if (EffectDictionary.ContainsKey(target))
        {
            EffectDictionary[target].Remove(effect);
            effect.RemoveEffect();
            if (EffectDictionary[target].Count > 0)//xóa khỏi dictionary nếu ko còn effect
            {
                EffectDictionary.Remove(target);
            }
        }
    }
    
    
    private void Update()
    {
        foreach (var pair in EffectDictionary)
        {
            if (!pair.Key)
            {
                EffectDictionary.Remove(pair.Key);
                continue;
            }
            List<EffectBase> effectsToRemove = new List<EffectBase>();
            foreach (EffectBase effect in pair.Value)
            {
                if (effect.UpdateEffect())
                {
                    effectsToRemove.Add(effect);
                }
            }

            foreach (EffectBase effect in effectsToRemove)
            {
                RemoveEffect(effect);
            }
        }
    }
    
    #endregion
}
