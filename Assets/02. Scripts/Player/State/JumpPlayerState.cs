using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPlayerState : PlayerState, IPlayerState
{
    public JumpPlayerState(PlayerController playerController, PlayerInput playerInput, Animator animator)
            :base(playerController, playerInput, animator)
    {

    }

    public void Enter()
    {
        _animator.SetTrigger(PlayerController.PlayerAniParamJump);

        
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
