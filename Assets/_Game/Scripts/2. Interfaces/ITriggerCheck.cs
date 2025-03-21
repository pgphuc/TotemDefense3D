using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheck

{
    bool IsOwnerInCheck { get; set; }
    void SetOwnerInCheckBool(bool isOwnerInCheck);
    bool IsOpponentInCheck { get; set; }
    void SetOpponentInCheckBool(bool isOpponentInCheck);
    void HandleEnter(Collider other);
    void HandleExit();
}
