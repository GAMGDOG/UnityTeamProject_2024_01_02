using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{
    public Monster Monster { get; }
    public PlayerHealth Target { get; private set; }
    public MonsterIdleState IdleState { get; }
    public MonsterChasingState ChasingState { get; }
    public MonsterWanderingState WanderingState { get; }
    public MonsterAttackState AttackState { get; }
    public MonsterDeadState DeadState { get; }
    public MonsterAwakeState AwakeState { get; }
    public Vector2 MonementInput { get; set; }
    public float MovementSpeed { get; } = 4f;
    public float RotationDamping { get; set; } = 1f;
    public float MovementSpeedModifier { get; set; } = 1f;
    
    public MonsterStateMachine(Monster monster)
    {
        Monster = monster;
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        IdleState = new MonsterIdleState(this);
        ChasingState = new MonsterChasingState(this);
        WanderingState = new MonsterWanderingState(this);
        AttackState = new MonsterAttackState(this);
        DeadState = new MonsterDeadState(this);
        AwakeState = new MonsterAwakeState(this);
    }
}
