using UnityEngine;

public class PrepareRoll : StateMachineBehaviour {
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<EnkiduController>().rollPrepared = true;
    }
}
