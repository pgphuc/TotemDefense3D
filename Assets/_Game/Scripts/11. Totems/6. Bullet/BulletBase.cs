using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : GameUnit
{
    private float flyDuration = 3f;
    public override void Awake()
    {
        base.Awake();
    }

    public override void StateMachineConstructor()
    {
        base.StateMachineConstructor();
    }

    public override void ComponentConstructor()
    {
        base.ComponentConstructor();
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void InitAllComponents()
    {
        base.InitAllComponents();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }


    private void Update()
    {
        
    }
    
    public IEnumerator ShootBullet(List<Vector3> points)
    {
        float stepTime = flyDuration / points.Count;
        foreach (Vector3 point in points)
        {
            transform.position = point;
            yield return new WaitForSeconds(stepTime);
        }
    }
}
