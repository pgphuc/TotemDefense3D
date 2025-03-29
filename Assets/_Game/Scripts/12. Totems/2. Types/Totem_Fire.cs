using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem_Fire : TotemBase
{
    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _attackComponent = new Component_Attack_Totem(this, _bulletSpawnPoint, 10f, 3f, 9f, 1.1f);
        components.Add(_attackComponent);
    }
}
