using UnityEngine;

public class StopAttack : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<EnkiduController>().attackStopped = true;
    }
}
