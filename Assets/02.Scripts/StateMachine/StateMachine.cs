using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine
{
    protected IState currentState;

    #region Methods

    public void ChangeState(IState newState)
    {
        if(currentState != newState)
        {
            currentState?.Exit();

            currentState = newState;

            currentState?.Enter();
        }
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    #endregion
}
