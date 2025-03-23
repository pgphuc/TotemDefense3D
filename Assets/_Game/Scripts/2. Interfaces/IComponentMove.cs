using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IComponentMove
{
    Vector3 _target { get; set; }
    Component_Health _dualingTarget { get; set; }
    float _meleeRadius { get; set; }
    void StartMoving();
    void StopMoving();
    void Moving();
    void SetMoveTarget(Vector3 target);
    Vector3 FindDestination();
}
