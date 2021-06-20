using UnityEngine;

public class SetAttackAllowance : StateMachineBehaviour {
    [SerializeField] bool allow;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.GetComponent<PlayerFighting>().SetCanAttack(allow);
    }
}
