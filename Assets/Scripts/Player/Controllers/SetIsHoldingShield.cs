using UnityEngine;

public class SetIsHoldingShield : StateMachineBehaviour {
    [SerializeField] bool hold;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<MoveSystem>().SetIsHoldingShield(hold);
    }
}
