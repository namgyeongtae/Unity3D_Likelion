using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState
{
    protected Animator _animator;
    protected PlayerInput _playerInput;
    protected PlayerController _playerController;

    public PlayerState(PlayerController playerController, PlayerInput playerInput, Animator animator)
    {
        _playerController = playerController;
        _playerInput = playerInput;
        _animator = animator;
    }

    // 캐릭터 회전
    protected void Rotate(float x, float z)
    {
        if (_playerInput.camera != null)
        {
            var cameraTr = _playerInput.camera.transform;
            var cameraForward = cameraTr.forward;
            var cameraRight = cameraTr.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            var moveDirection = cameraForward * z + cameraRight * x;

            if (moveDirection != Vector3.zero)
            {
                moveDirection.Normalize();
                _playerController.transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }
    }

    protected void Jump(InputAction.CallbackContext context)
    {
        _playerController.Jump();
        _playerController.ChangeState(PlayerController.EPlayerState.Jump);
    }

    protected void Attack(InputAction.CallbackContext context)
    {
        _playerController.ChangeState(PlayerController.EPlayerState.Attack);
    }
}
