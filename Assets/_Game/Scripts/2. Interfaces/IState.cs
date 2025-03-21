using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter();//Gọi khi enter 1 state
    void OnExit();//Gọi khi kết thúc 1 state
    void OnFrameUpdate();//Gọi trong update để thực hiện state
    void OnPhysicsUpdate();//Gọi trong fixedUpdate để thực hiện tác động vật lý
    void AnimationTriggerEvent(GameUnit.AnimationTriggerType triggerType); //Được gọi khi chạy animation
}
