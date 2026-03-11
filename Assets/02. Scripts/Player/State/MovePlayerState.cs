using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayerState : PlayerState, ICharacterState
{
    private float _moveSpeed;

    public MovePlayerState(PlayerController playerController, PlayerInput playerInput, Animator animator) 
        : base(playerController, playerInput, animator)
    {
    }

    public void Enter()
    {
        // Move 애니메이션 실행
        _animator.SetBool(PlayerController.PlayerAniParamMove, true);

        // _moveSpeed 초기화
        _moveSpeed = 0f;

        _playerInput.actions["Jump"].performed += Jump;
        _playerInput.actions["Fire"].performed += Attack;
    }

    public void Update()
    {
        var moveVector = _playerInput.actions["Move"].ReadValue<Vector2>();
        if (moveVector != Vector2.zero)
        {
            // TODO: 캐릭터 회전
            Rotate(moveVector.x, moveVector.y);
        }
        else
        {
            _playerController.ChangeState(PlayerController.EPlayerState.Idle);
        }

        // 이동 스피드 설정
        bool isRun = _playerInput.actions["Run"].IsPressed();
        if (isRun && _moveSpeed < 1f)
        {
            _moveSpeed += Time.deltaTime;
            _moveSpeed = Mathf.Clamp01(_moveSpeed);
        }
        else if (!isRun && _moveSpeed > 0f)
        {
            _moveSpeed -= Time.deltaTime;
            _moveSpeed = Mathf.Clamp01(_moveSpeed);
        }

        _animator.SetFloat(PlayerController.PlayerAniParamMoveSpeed, _moveSpeed);
    }

    public void Exit()
    {
        // Move 애니메이션 중단
        _animator.SetBool(PlayerController.PlayerAniParamMove, false);
    
        _playerInput.actions["Jump"].performed -= Jump;
        _playerInput.actions["Fire"].performed -= Attack;
    }
}
