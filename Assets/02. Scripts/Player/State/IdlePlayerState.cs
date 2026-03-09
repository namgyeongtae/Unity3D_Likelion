using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class IdlePlayerState : PlayerState, IPlayerState
{
    public IdlePlayerState(PlayerController playerController, PlayerInput playerInput, Animator animator) 
        : base(playerController, playerInput, animator) { }

    public void Enter()
    {
        Debug.Log("##Idle Enter");
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
        Debug.Log("##Idle Exit");
    }
}
