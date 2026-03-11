using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemyState : EnemyState, ICharacterState
{
    public PatrolEnemyState(EnemyController enemyController, Animator animator, NavMeshAgent navMeshAgent) 
        : base(enemyController, animator, navMeshAgent) { }

    public void Enter() 
    { 
        _animator.SetBool(EnemyController.EnemyAniParamPatrol, true);
    }

    public void Update() 
    {
        var detectionTargetTransform = _enemyController.DetectionTargetInCircle();

        if (detectionTargetTransform != null)
        {
            _navMeshAgent.SetDestination(detectionTargetTransform.position);
            _enemyController.ChangeState(EnemyController.EEnemyState.Chase);
        }
        // Patrol 상태에서 목적지에 도착했을 경우, Idle로 전환
        else if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _enemyController.ChangeState(EnemyController.EEnemyState.Idle);
        }
    }

    public void Exit() 
    { 
        _animator.SetBool(EnemyController.EnemyAniParamPatrol, false);
    }
}
