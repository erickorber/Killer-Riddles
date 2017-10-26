using UnityEngine;

public class TwoHandedAttackBehaviour : StateMachineBehaviour {
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        //Temporarily disable this feature
        animator.SetBool("bothHands", false);
	}

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.1)
        {
            //Re-enable this feature
            animator.SetBool("bothHands", true);
        }
    }
}
