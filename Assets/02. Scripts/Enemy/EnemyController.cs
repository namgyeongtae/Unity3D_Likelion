using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    // AI 관련
    [SerializeField] private float _patrolDetectionDistance = 10f;
    [SerializeField] private float _patrolWaitTime = 1f;
    [SerializeField] private float _patrolRandomChance = 30f;

    [SerializeField] private LayerMask _detectionTargetLayerMask;
    [SerializeField] private float detectionSightAngle = 30f;
    [SerializeField] private float minimumRunDistance = 1f;

    private Transform _targetTransform;
    private Collider[] _detectionResults = new Collider[1];

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    public enum EEnemyState
    {
        None, Idle, Patrol, Chase, Attack, Hit, Dead
    }

    public EEnemyState State { get; private set; }
    private Dictionary<EEnemyState, ICharacterState> _enemyStates;

    public float PatrolDetectionDistance => _patrolDetectionDistance;
    public float PatrolWaitTime => _patrolWaitTime;
    public float PatrolRandomChance => _patrolRandomChance;
    public float DetectionSightAngle => detectionSightAngle;

    public float MinimumRunDistance => minimumRunDistance;

    // 애니메이터 파라미터
    public static readonly int EnemyAniParamIdle = Animator.StringToHash("idle");
    public static readonly int EnemyAniParamPatrol = Animator.StringToHash("patrol");
    public static readonly int EnemyAniParamChase = Animator.StringToHash("chase");
    public static readonly int EnemyAniParamAttack = Animator.StringToHash("attack");
    public static readonly int EnemyAniParamHit = Animator.StringToHash("hit");
    public static readonly int EnemyAniParamDead = Animator.StringToHash("dead");
    public static readonly int EnemyAniParamMoveSpeed = Animator.StringToHash("move_speed");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        var idleEnemyState = new IdleEnemyState(this, _animator, _navMeshAgent);
        var patrolEnemyState = new PatrolEnemyState(this, _animator, _navMeshAgent);
        var chaseEnemyState = new ChaseEnemyState(this, _animator, _navMeshAgent);
        /* var attackEnemyState = new AttackEnemyState(this, _animator, _navMeshAgent);
        var hitEnemyState = new HitEnemyState(this, _animator, _navMeshAgent);
        var deadEnemyState = new DeadEnemyState(this, _animator, _navMeshAgent); */

        _enemyStates = new Dictionary<EEnemyState, ICharacterState>
        {
            { EEnemyState.None, null },
            { EEnemyState.Idle, idleEnemyState },
            { EEnemyState.Patrol, patrolEnemyState },
            { EEnemyState.Chase, chaseEnemyState },
        };

        ChangeState(EEnemyState.Idle);

        // 추격 정보 초기화
        _targetTransform = null;
    }

    void Update()
    {
        if (State != EEnemyState.None)
        {
            _enemyStates[State].Update();
        }
    }

    public void ChangeState(EEnemyState newState)
    {
        if (newState == EEnemyState.None) return;
        if (State == newState) return;

        _enemyStates[State]?.Exit();
        State = newState;
        _enemyStates[State]?.Enter();
    }

    private void OnAnimatorMove()
    {
        var position = _animator.rootPosition;
        _navMeshAgent.nextPosition = position;
        transform.position = position;
    }

    public Transform DetectionTargetInCircle()
    {
        if (_targetTransform == null)
        {
            // _targetTransform이 없으면 새롭게 찾기기
            Physics.OverlapSphereNonAlloc(transform.position, _patrolDetectionDistance, _detectionResults, _detectionTargetLayerMask);

            // detectionResults 배열 0번 인덱스에 값이 있다면 그걸 _targetTransform에 할당
            _targetTransform = _detectionResults[0]?.transform;
        }
        else
        {
            // _targetTransform이 있으면, 그 대상과의 거리를 계산해서 정해진 거리를 벗어나면 _targetTransform 정보 초기화
            float playerDistance = Vector3.Distance(transform.position, _targetTransform.position);
            if (playerDistance > _patrolDetectionDistance)
            {
                _targetTransform = null;
                _detectionResults[0] = null;
            }
        }

        return _targetTransform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PatrolDetectionDistance);

        Gizmos.color = Color.red;
        Vector3 rightDirection = Quaternion.Euler(0, detectionSightAngle, 0) * transform.forward;
        Vector3 leftDirection = Quaternion.Euler(0, -detectionSightAngle, 0) * transform.forward;

        Gizmos.DrawRay(transform.position, rightDirection * PatrolDetectionDistance);
        Gizmos.DrawRay(transform.position, leftDirection * PatrolDetectionDistance);
        Gizmos.DrawRay(transform.position, transform.forward * PatrolDetectionDistance);

        if (_navMeshAgent && _navMeshAgent.hasPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_navMeshAgent.destination, 0.5f);
            Gizmos.DrawLine(transform.position, _navMeshAgent.destination);
        }
    }
}
