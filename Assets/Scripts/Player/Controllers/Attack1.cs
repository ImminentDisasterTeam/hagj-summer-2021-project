using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("djfhgjdkls;fkjghfdkls;dkfjh");
        animator.GetComponent<PlayerFighting>().ComboAttack1();
    }
}