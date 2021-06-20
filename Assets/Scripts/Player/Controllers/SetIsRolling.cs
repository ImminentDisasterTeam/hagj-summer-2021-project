using UnityEngine;

public class SetIsRolling : StateMachineBehaviour {
    [SerializeField] bool isRolling;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerController>().SetIsRolling(isRolling);
    }
}

