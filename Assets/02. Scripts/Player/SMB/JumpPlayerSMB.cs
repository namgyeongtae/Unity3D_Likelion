using UnityEngine;

public class JumpPlayerSMB : StateMachineBehaviour
{
    private PlayerController _playerController;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerController == null) _playerController = animator.GetComponent<PlayerController>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController.ChangeState(PlayerController.EPlayerState.Idle);
    }
}
