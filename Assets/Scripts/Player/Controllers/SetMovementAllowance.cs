using UnityEngine;

public class SetMovementAllowance : StateMachineBehaviour {
    [SerializeField] bool allow;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerController>().CanMove = allow;
    }
}
