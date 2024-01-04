using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterBaseState : IState
{
    protected MonsterStateMachine stateMachine;

    #region IState
    public MonsterBaseState(MonsterStateMachine enemyStateMachine)
    {
        stateMachine = enemyStateMachine;
    }
    public virtual void Enter()
    {
        stateMachine.Monster.Agent.speed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
    }
    public virtual void Exit()
    {
    }
    public virtual void Update()
    {
        if (!stateMachine.Monster.health.IsDead)
            Move();
        else
            stateMachine.ChangeState(stateMachine.DeadState);

    }
    public virtual void HandleInput()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }
    #endregion
    #region Methods
    protected void StartAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.Monster.Animator.SetBool(animationHash, false);
    }
    private void Move()
    {
        stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);
        Rotate(stateMachine.Target.transform.position - stateMachine.Monster.transform.position);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            stateMachine.Monster.transform.rotation = Quaternion.Slerp(stateMachine.Monster.transform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }
    protected bool IsInChaseRange()
    {
        if (stateMachine.Target.IsDead)
        {
            stateMachine.Monster.enabled = false;
            return false;
        }
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Monster.Data.PlayerChasingRange * stateMachine.Monster.Data.PlayerChasingRange;
    }
    protected bool IsInAttackRange()
    {
        if (stateMachine.Target.IsDead)
        {
            stateMachine.Monster.enabled = false;
            return false;
        }
        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Monster.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Monster.Data.AttackRange * stateMachine.Monster.Data.AttackRange;
    }
    #endregion
}
