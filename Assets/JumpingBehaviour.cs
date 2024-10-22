using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (player.instance.isOnTheWall == true)
        {
            player.instance.animator.Play("WallSliding");
        }

        if (player.instance.taPulando)
        {
            player.instance.animator.Play("Pulo");
            player.instance.taPulando = false;
        }

        if (player.instance.estaDashando && player.instance.taNoChao == false)
        {
            player.instance.animator.Play("AirDash");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.instance.taNoChao == false)
        {
            player.instance.animator.Play("Ataque3");
            Espatula.instance.AtivarCollider3();
            player.instance.estaAtacando = false;
            Espatula.instance.damage = 1000;
        }

    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.instance.taPulando = false;
        player.instance.aterrizando = true;
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
