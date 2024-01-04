using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterAnimationData
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string AttackParameterName = "Attack";
    [SerializeField] private string WalkParameterName = "Walk";
    [SerializeField] private string DeadParameterName = "Dead";
    [SerializeField] private string AwakeParameterName = "Awake";

    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int DeadParameterHash { get; private set; }
    public int AwakeParameterHash { get; private set; }


    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(AttackParameterName);
        WalkParameterHash = Animator.StringToHash(WalkParameterName);
        DeadParameterHash = Animator.StringToHash(DeadParameterName);
        AwakeParameterHash = Animator.StringToHash(AwakeParameterName);
    }
}
