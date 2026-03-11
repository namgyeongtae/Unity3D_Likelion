using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class IdlePlayerState : PlayerState, ICharacterState
{
    public IdlePlayerState(PlayerController playerController, PlayerInput playerInput, Animator animator) 
        : base(playerController, playerInput, animator) { }

    public void Enter()
    {
        // Debug.Log("##Idle Enter");
        _animator.SetBool(PlayerController.PlayerAniParamIdle, true);

        // �׼� �Ҵ�
        _playerInput.actions["Jump"].performed += Jump;
        _playerInput.actions["Fire"].performed += Attack;
    }

    public void Update()
    {
        if (_playerInput.actions["Move"].IsPressed())
        {
            _playerController.ChangeState(EPlayerState.Move);
        }
    }

    public void Exit()
    {
        _animator.SetBool(PlayerController.PlayerAniParamIdle, false);

        _playerInput.actions["Jump"].performed -= Jump;
        _playerInput.actions["Fire"].performed -= Attack;
    }
}
