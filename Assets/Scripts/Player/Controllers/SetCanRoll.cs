using UnityEngine;

public class SetCanRoll : StateMachineBehaviour {
    [SerializeField] bool canRoll;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerController>().CanRoll = canRoll;
    }
}

