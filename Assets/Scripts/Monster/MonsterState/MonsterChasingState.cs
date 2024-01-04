using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterChasingState : MonsterBaseState
{
    public MonsterChasingState(MonsterStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1.5f;
        base.Enter();
        StartAnimation(stateMachine.Monster.AnimationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Monster.AnimationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.WanderingState);
            return;
        }
        else if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }

}
