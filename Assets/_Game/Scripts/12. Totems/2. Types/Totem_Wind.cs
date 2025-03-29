using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem_Wind : TotemBase
{
    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _attackComponent = new Component_Attack_Totem(this, _bulletSpawnPoint, 5f, 1.5f, 5f, 1.3f);
        components.Add(_attackComponent);
    }
   
}
