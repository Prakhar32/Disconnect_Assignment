using UnityEngine;

public class IdleState : CouplingState
{
    private CouplingStateMachine _statemachine;
    internal IdleState(CouplingStateMachine stateMachine)
    {
        _statemachine = stateMachine;
    }

    public void OnEnterState()
    {
    }

    public void Attach()
    {
        _statemachine.SetState(_statemachine._attachmentState);
    }

    public void Detach()
    {
    }
}
