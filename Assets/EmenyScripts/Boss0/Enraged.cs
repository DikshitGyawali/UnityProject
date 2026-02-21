using UnityEngine;

public class Enraged : StateMachineBehaviour
{
    private EyeManipulation eye;
    //private PlayerHealth playerHealth;
    private PlayerState playerState;
    private Boss0 bossScript;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eye = animator.GetComponent<EyeManipulation>();
        bossScript = animator.GetComponent<Boss0>();
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        playerState.CanControl = false;
        playerState.rb.linearVelocity = Vector2.zero;
        playerState.CanAttack = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       playerState.CanControl = true;
       playerState.CanAttack = true;
       eye.eyeInControl = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
