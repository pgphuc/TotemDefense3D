using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem_Ice : TotemBase
{
    protected override void ComponentConstructor()
    {
        base.ComponentConstructor();
        _attackComponent = new Component_Attack_Totem(this, _bulletSpawnPoint, 3f, 1f, 10f, 1.3f);
        components.Add(_attackComponent);
    }
}
