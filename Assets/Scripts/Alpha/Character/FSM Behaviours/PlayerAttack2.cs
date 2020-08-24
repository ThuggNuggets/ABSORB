using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2 : StateMachineBehaviour
{
    PlayerHandler _playerHandler;
    CombatHandler _combatHandler;
    InputManager _inputManager;
    int index;

    void Awake()
    {
        _playerHandler = FindObjectOfType<PlayerHandler>();
        index = 0;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _combatHandler = _playerHandler.GetCombatHandler();
        _inputManager = _playerHandler.GetInputManager();
        // Setting the current state within the player handler
        _playerHandler.SetState(PlayerHandler.PlayerAnimatorState.ATTACK);
        // if (index == 2)
        // {
        //     animator.SetBool("Attack" + index, true);
        //     index = 0;
        //     // _combatHandler.ResetAttackTimer();
        // }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_inputManager.GetAttackButtonPress() && _combatHandler.shieldState != CombatHandler.ShieldState.Shielding && _combatHandler.GetAttackTimer() >= _combatHandler.minTimeBetweenAttack)
        {
            _combatHandler.ResetAttackTimer();
            _combatHandler.attackIndex++;
            animator.SetBool("Attack3", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
