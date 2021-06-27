using UnityEngine;

public class EnkiduUpgrade : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<EnkiduController>().poweruped = true;
    }
}
