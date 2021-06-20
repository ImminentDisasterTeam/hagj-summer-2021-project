using UnityEngine;

public class ForbidAttack : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerFighting>().SetCanAttack(false);
    }
}
