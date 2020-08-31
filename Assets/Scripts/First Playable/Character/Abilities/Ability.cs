using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected AbilityHandler abilityHandler;
    protected bool active;
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Activate();
}
