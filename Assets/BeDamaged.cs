using UnityEngine;

public class BeDamaged : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<EnkiduController>().finishedBeingDamaged = true;
    }
}
