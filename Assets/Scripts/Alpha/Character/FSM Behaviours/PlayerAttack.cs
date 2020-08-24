using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    PlayerHandler _playerHandler;
    CombatHandler _combatHandler;

    void Awake()
    {
        _playerHandler = FindObjectOfType<PlayerHandler>();
        _combatHandler = _playerHandler.GetComponent<CombatHandler>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Setting the current state within the player handler
        _playerHandler.SetState(PlayerHandler.PlayerAnimatorState.ATTACK);

        switch (_combatHandler.attackState)
        {
            case CombatHandler.AttackState.Attack1:
                animator.SetBool("Attack", true);
                _combatHandler.ResetAttackTimer();
                break;
            case CombatHandler.AttackState.Attack2:
                animator.SetBool("Attack2", true);
                _combatHandler.ResetAttackTimer();
                break;
            case CombatHandler.AttackState.Attack3:
                animator.SetBool("Attack3", true);
                _combatHandler.AttackComboFinish();
                break;
            default:
                break;
        }
    }

    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {

    // }

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
