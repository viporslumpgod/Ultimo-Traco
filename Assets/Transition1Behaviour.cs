using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition1Behaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.instance.podeAtacar = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.instance.estaAtacando)
        {
            player.instance.animator.Play("Ataque2");
            Espatula.instance.AtivarCollider2();
            float ataqueProjecao = 20f; // Ajusta esse valor para o quanto você quer que ele se mova
            player.instance.rb.velocity = new Vector2(player.instance.horizontal * ataqueProjecao, player.instance.rb.velocity.y);
            //player.instance.InputManager();
            player.instance.podeMover = false;
            player.instance.podeAtacar = true;

        }

        if(player.instance.estaAndando)
        {
            player.instance.animator.Play("Walk");
        }

        
       
    }
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            player.instance.estaAtacando = false;
            player.instance.podeMover = true;
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

