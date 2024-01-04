using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public PlayerInput Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private PlayerStateMachine stateMachine;

    public PlayerStamina Stamina { get; private set; }

    [field: SerializeField] public Gun Gun { get; private set; }
    [field: SerializeField] public GunController GunController { get; private set; }

    // 현재 가지고 있는 키 확인
    [HideInInspector]
    public KeyCheck KeyCheck = new();

    private void Awake()
    {
        Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Stamina = GetComponent<PlayerStamina>();

        stateMachine = new PlayerStateMachine(this, Stamina);

        //Main에 이 플레이어 추가 - 연호
        Main.Player = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}

//잠시 넣어놓았습니다 - 연호
public class KeyCheck
{
    public bool Red { get; set; }
    public bool Blue { get; set; }
    public bool Green { get; set; }
}
