using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    //public AIBrain aiBrain;

    //[Header("Detect")]
    //public float detectionDistance;

    //private enum STATE
    //{
    //    IDLE,
    //    MOVE,
    //    ATTACK,
    //    DEATH,
    //    COUNT,
    //}

    //private void Awake()
    //{
    //    AIBehaviour[] behaviours = new AIBehaviour[(int)STATE.COUNT];
    //    behaviours[(int) STATE.IDLE]    = new Minion_Idle();
    //    behaviours[(int) STATE.MOVE]    = new Minion_Move();
    //    behaviours[(int) STATE.ATTACK]  = new Minion_Attack();
    //    behaviours[(int) STATE.DEATH]   = new Minion_Death();

    //    foreach(AIBehaviour aib in behaviours)
    //    {
    //        aib.InitialiseState(aiBrain);
    //        aiBrain.AddBehaviour(aib);
    //    }
    //}

    //private class Minion_Idle : AIBehaviour
    //{
    //    public void Awake()
    //    {
    //        id = (int) STATE.IDLE;
    //    }

    //    public override void OnEnter()
    //    {
    //    }

    //    public override void OnExit()
    //    {
    //    }

    //    public override void OnFixedUpdate()
    //    {
    //    }

    //    public override void OnUpdate()
    //    {
    //        //if (brain.GetDistanceToPlayer() < detectionDistance)
    //        //    brain.SetState(E_Behaviours.CHASE);
    //    }
    //}

    //private class Minion_Move : AIBehaviour
    //{
    //    public void Awake()
    //    {
    //        id = (int)STATE.MOVE;
    //    }

    //    public override void OnEnter()
    //    {
    //    }

    //    public override void OnExit()
    //    {
    //    }

    //    public override void OnFixedUpdate()
    //    {
    //    }

    //    public override void OnUpdate()
    //    {
    //    }
    //}

    //private class Minion_Attack : AIBehaviour
    //{
    //    public void Awake()
    //    {
    //        id = (int)STATE.ATTACK;
    //    }

    //    public override void OnEnter()
    //    {
    //    }

    //    public override void OnExit()
    //    {
    //    }

    //    public override void OnFixedUpdate()
    //    {
    //    }

    //    public override void OnUpdate()
    //    {
    //    }
    //}

    //private class Minion_Death : AIBehaviour
    //{
    //    public void Awake()
    //    {
    //        id = (int)STATE.DEATH;
    //    }

    //    public override void OnEnter()
    //    {
    //    }

    //    public override void OnExit()
    //    {
    //    }

    //    public override void OnFixedUpdate()
    //    {
    //    }

    //    public override void OnUpdate()
    //    {
    //    }
    //}

}
