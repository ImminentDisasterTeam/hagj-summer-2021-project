using UnityEngine;

public class SetStanned : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<MoveSystem>().SetIsHoldingShield(false);
        animator.GetComponent<PlayerFighting>().SetCanAttack(false);
        animator.GetComponent<PlayerController>().SetIsRolling(false);
        animator.GetComponent<PlayerController>().SetCanRoll(false);
        animator.GetComponent<PlayerController>().SetCanMove(false);

    }
}
