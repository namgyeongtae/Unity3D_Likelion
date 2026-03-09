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

    public void Update()
    {
        // Ground Distance 업데이트
        var playerPosition = _playerController.transform.position;
        var distance = CharacterUtil.GetDistanceFromGround(playerPosition, Constants.GroundLayerMask, 10f);
    
        _animator.SetFloat(PlayerController.PlayerAniParamGround, distance);

        Debug.DrawRay(playerPosition, Vector3.down * 10f, Color.red);
    }

    public void Exit()
    {
    }
}
