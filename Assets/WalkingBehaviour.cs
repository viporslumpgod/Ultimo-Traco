using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.instance.estaAndando == true)
        {
            player.instance.animator.Play("Walk");
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.instance.estaAndando && player.instance.taNoChao)
        {
            player.instance.animator.Play("Walk");
        }

        if (player.instance.estaAndando && player.instance.taNoChao == false)
        {
            player.instance.animator.Play("Falling");
        }

        if (player.instance.isOnTheWall == true)
        {
            player.instance.animator.Play("WallSliding");
        }

        if (player.instance.taPulando == true)
        {
            player.instance.animator.Play("Pulo");
        }

        if (player.instance.estaDashando && player.instance.taNoChao)
        {
            player.instance.animator.Play("Dash");
        }

        if (player.instance.estaDashando && player.instance.taNoChao == false)
        {
            player.instance.animator.Play("AirDash");
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
