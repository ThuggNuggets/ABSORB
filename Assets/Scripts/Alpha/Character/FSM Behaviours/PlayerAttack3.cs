﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack3 : StateMachineBehaviour
{
    // public string parameterName;
    // public int value;

    private PlayerHandler _playerHandler;
    private CombatHandler _combatHandler;

    void Awake()
    {
        _playerHandler = FindObjectOfType<PlayerHandler>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatHandler = _playerHandler.GetComponent<CombatHandler>();
        // Setting the current state within the player handler
        _playerHandler.SetState(PlayerHandler.PlayerAnimatorState.ATTACK);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Call the reset function upon animation finish
        _combatHandler.AttackComboFinish();
    }
}
