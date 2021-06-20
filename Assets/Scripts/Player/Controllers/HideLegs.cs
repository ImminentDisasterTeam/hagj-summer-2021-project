using UnityEngine;

public class HideLegs : StateMachineBehaviour {
    [SerializeField] bool hide;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("isHidingLegs", hide);
    }
}
