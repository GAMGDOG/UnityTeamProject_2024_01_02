using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDeadState : MonsterBaseState
{
    public MonsterDeadState(MonsterStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Monster.AnimationData.DeadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Monster.AnimationData.DeadParameterHash);
    }
    public override void Update()
    {
        if (stateMachine.Monster.health.Health == 100)
        {
            stateMachine.Monster.health.IsDead = false;
            stateMachine.ChangeState(stateMachine.AwakeState);
        }
    }
}
