using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem_Lightning : TotemBase
{
    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _attackComponent = new Component_Attack_Totem(this, _bulletSpawnPoint, 2f, 2f, 100f, 3f);
        components.Add(_attackComponent);                                  
    }
    
}
