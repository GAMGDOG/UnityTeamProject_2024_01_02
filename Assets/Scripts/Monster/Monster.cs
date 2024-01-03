using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public MonsterSO Data { get; private set; }
    [field: Header("Animations")]
    [field: SerializeField] public MonsterAnimationData AnimationData { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private MonsterStateMachine stateMachine;

    private void Awake()
    {
        AnimationData.Initialize();

        Agent = GetComponent<NavMeshAgent>();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponentInChildren<CharacterController>();

        stateMachine = new MonsterStateMachine(this);
    }
}
