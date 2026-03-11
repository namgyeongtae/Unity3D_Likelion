using UnityEngine;
using UnityEngine.InputSystem;

public class AttackPlayerState : PlayerState, ICharacterState
{
    public AttackPlayerState(PlayerController playerController, PlayerInput playerInput, Animator animator) 
        : base(playerController, playerInput, animator)
    {
    }

    public void Enter()
    {
        _animator.SetTrigger(PlayerController.PlayerAniParamAttack);
        _playerInput.actions["Fire"].performed += AttackTrigger;
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        _playerInput.actions["Fire"].performed -= AttackTrigger;
    }

    private void AttackTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("##AttackTrigger");
        _animator.SetTrigger(PlayerController.PlayerAniParamAttack);
    }
}
