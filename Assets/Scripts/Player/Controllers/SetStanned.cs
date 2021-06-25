using UnityEngine;

public class SetStanned : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<MoveSystem>().isHoldingShield = false;
        animator.GetComponent<PlayerFighting>().canAttack = false;
        animator.GetComponent<PlayerController>().IsRolling = false;
        animator.GetComponent<PlayerController>().CanRoll = false;
        animator.GetComponent<PlayerController>().CanMove = false;
    }
}
