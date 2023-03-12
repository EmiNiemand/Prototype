using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    void OnEnter(GameObject player);
    void OnUse(GameObject player);
    void OnExit(GameObject player);
}
