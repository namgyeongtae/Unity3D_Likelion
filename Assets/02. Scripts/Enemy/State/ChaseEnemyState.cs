using UnityEngine;
using UnityEngine.AI;

public class ChaseEnemyState : EnemyState, ICharacterState
{
    public ChaseEnemyState(EnemyController enemyController, Animator animator, NavMeshAgent navMeshAgent) 
        : base(enemyController, animator, navMeshAgent) { }

    public void Enter() 
    { 
        _animator.SetBool(EnemyController.EnemyAniParamChase, true);
    }

    public void Update() 
    {
        var detectionTargetTransform = _enemyController.DetectionTargetInCircle();
        if (detectionTargetTransform != null)
        {
            // 공격 여부 판단
            if (!_navMeshAgent.pathPending && 
                _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _enemyController.ChangeState(EnemyController.EEnemyState.Attack);
            }

            // 달리기 여부 판단
            if (DetectionTargetInSight(detectionTargetTransform.position) 
                && _navMeshAgent.remainingDistance > _enemyController.MinimumRunDistance)
            {
                _animator.SetFloat(EnemyController.EnemyAniParamMoveSpeed, 1f);
            }
            else
            {
                _animator.SetFloat(EnemyController.EnemyAniParamMoveSpeed, 0f);
            }

            _navMeshAgent.SetDestination(detectionTargetTransform.position);
        }
        else
        {
            _enemyController.ChangeState(EnemyController.EEnemyState.Idle);
        }
    }

    public void Exit() 
    { 
        _animator.SetBool(EnemyController.EnemyAniParamChase, false);
    }

    private bool DetectionTargetInSight(Vector3 position)
    {
        var cosTheta = Vector3.Dot(_enemyController.transform.forward, 
            (position - _enemyController.transform.position).normalized);
        var angle = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;

        return angle < _enemyController.DetectionSightAngle * 0.5f;
    }
}
