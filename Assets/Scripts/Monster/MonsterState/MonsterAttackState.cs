using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    private bool alreadyAppliedForce;
    private bool alreadyAppliedDealing;
    public MonsterAttackState(MonsterStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override async void Enter()
    {
        alreadyAppliedForce = false;
        alreadyAppliedDealing = false;

        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
        if(!alreadyAppliedDealing)
        {
            await Task.Delay(3000);
            alreadyAppliedDealing = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        TryApplyForce();

        if(alreadyAppliedDealing)
        {
            if (IsInChaseRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.WanderingState);
                return;
            }
        }
    }
    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;
            
        stateMachine.Monster.ForceReceiver.Reset();
        stateMachine.Monster.ForceReceiver.AddForce(stateMachine.Monster.transform.forward * stateMachine.Monster.Data.Force);
    }
}
