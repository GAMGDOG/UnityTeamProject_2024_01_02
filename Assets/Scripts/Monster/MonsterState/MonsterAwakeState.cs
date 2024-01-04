using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAwakeState : MonsterBaseState
{
    public MonsterAwakeState(MonsterStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Monster.AnimationData.AwakeParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Monster.AnimationData.AwakeParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if (IsInChaseRange())
            stateMachine.ChangeState(stateMachine.ChasingState);
        else if (IsInAttackRange())
            stateMachine.ChangeState(stateMachine.AttackState);
        else
            stateMachine.ChangeState(stateMachine.WanderingState);
    }



}
