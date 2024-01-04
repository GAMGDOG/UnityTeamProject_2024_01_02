using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundedState
{
    private PlayerStamina playerStamina;

    public PlayerWalkState(PlayerStateMachine playerStateMachine, PlayerStamina stamina) : base(playerStateMachine)
    {
        playerStamina = stamina;
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();

        stateMachine.Player.Gun.Anim.SetBool("Walk", true);
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.Player.Gun.Anim.SetBool("Walk", false);
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        // 달리기 상태로 전환하기 위한 충분한 스태미나가 있는지 확인
        if (playerStamina.CanConsumeStamina(playerStamina.StaminaConsumptionRate) && stateMachine.Player.GunController.IsFindSightMode == false)
        {
            base.OnRunStarted(context);
            stateMachine.ChangeState(stateMachine.RunState);
        }
    }
}
