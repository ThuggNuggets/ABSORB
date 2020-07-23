using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbsorbed : AIBehaviour
{

    /*
     * 
     *  Currently just destroying the object. Will implement the shader manipulation in here somewhere.
     * 
     * 
     */

    public override void OnEnter()
    {
        Destroy(this.gameObject);
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }
}
