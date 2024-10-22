using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.instance.estaAtacando)
        {
            player.instance.animator.Play("Ataque1");
            Espatula.instance.AtivarCollider1();
            player.instance.estaAtacando = false;
            Espatula.instance.damage = 1000;
        }

        if (player.instance.taPulando == true)
        {
            player.instance.animator.Play("Pulo");
        }

        if (player.instance.taNoChao == false)
        {
            player.instance.animator.Play("Falling");
        }

        if (player.instance.isOnTheWall == true)
        {
            player.instance.animator.Play("WallSliding");
        }

        if (player.instance.taNoChao == true)
        {
            player.instance.taNoChao = true;
            player.instance.animator.Play("Idle");


        }

        if (player.instance.estaDashando && player.instance.taNoChao)
        {
            player.instance.animator.Play("Dash");
        }

        

        if (player.instance.isCrouching)
        {
            player.instance.animator.Play("Crouch");
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.instance.estaAtacando = false;
        
    }

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
