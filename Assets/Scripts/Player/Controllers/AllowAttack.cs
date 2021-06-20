using UnityEngine;

public class AllowAttack : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerFighting>().SetCanAttack(true);
    }
}
