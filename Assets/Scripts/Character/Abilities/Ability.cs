using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected AbilityManager abilityManager;
    protected float damage;
    protected bool active;

    public void InitialiseAbility(AbilityManager abilityManager)
    {
        this.abilityManager = abilityManager;
    }

    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void Activate();
    public abstract void Deactivate();

    public float GetDamage()
    {
        return damage;
    }

    public bool Active
    {
        get { return active;  }
        set { active = value; }
    }
}
