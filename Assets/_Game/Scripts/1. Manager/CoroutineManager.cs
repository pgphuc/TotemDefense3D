using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : Singleton<CoroutineManager>
{
    public static Coroutine StartRoutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }
}
