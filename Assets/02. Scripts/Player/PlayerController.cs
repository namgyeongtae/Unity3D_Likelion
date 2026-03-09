using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _headTransform;

    private Animator _animator;
    private PlayerInput _playerInput;
    private CharacterController _cc;


    // 애니메이션 키
    public static readonly int PlayerAniParamJump = Animator.StringToHash("jump");
    public static readonly int PlayerAniParamIdle = Animator.StringToHash("idle");
    public static readonly int PlayerAniParamMove = Animator.StringToHash("move");
    public static readonly int PlayerAniParamMoveSpeed = Animator.StringToHash("move_speed");

    public enum EPlayerState
    {
        None, Idle, Move, Jump
    }

    // 현재 상태
    public EPlayerState PlayerState { get; private set; }

    // 상태와 상태 객체를 담고 있는 Dictionary
    private Dictionary<EPlayerState, IPlayerState> _playerStates = new();

    void Awake()
    {
        // 컴포넌트 초기화
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _cc = GetComponent<CharacterController>();

        // 상태 객체 초기화
        _playerStates.Add(EPlayerState.Idle, new IdlePlayerState(this, _playerInput, _animator));
        _playerStates.Add(EPlayerState.Move, new MovePlayerState(this, _playerInput, _animator));
        _playerStates.Add(EPlayerState.Jump, new JumpPlayerState(this, _playerInput, _animator));
        
        var playerCamera = Camera.main;
        if (playerCamera != null)
        {
            _playerInput.camera = playerCamera;
            playerCamera.GetComponent<CameraController>().SetTarget(_headTransform, _playerInput);
        }
    }

    void Start()
    {
        PlayerState = EPlayerState.Idle;
    }

    void Update()
    {
        if (PlayerState != EPlayerState.None)
        {
            _playerStates[PlayerState].Update();
        }
    }

    // 새로운 상태를 할당하는 함수
    public void ChangeState(EPlayerState newState)
    {
        Debug.Assert(newState != EPlayerState.None, "상태는 None이 될 수 없습니다.");
        Debug.Assert(_playerStates.ContainsKey(newState), "상태가 존재하지 않습니다.");
        Debug.Assert(PlayerState != newState, "이미 해당 상태입니다.");

        _playerStates[PlayerState].Exit();
        PlayerState = newState;
        _playerStates[PlayerState].Enter();

        Debug.Log($"상태 변경: {PlayerState}");
    }
}
