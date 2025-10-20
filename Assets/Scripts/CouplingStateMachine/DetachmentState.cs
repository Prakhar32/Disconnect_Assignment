using UnityEngine;

public class DetachmentState : CouplingState
{
    private CouplingStateMachine _statemachine;
    private Animator _animator;

    internal DetachmentState(CouplingStateMachine stateMachine, Animator animator)
    {
        _statemachine = stateMachine;
        _animator = animator;
    }

    public void Attach()
    {
    }

    public void Detach()
    {
    }

    public void OnEnterState()
    {
        _animator.SetBool("Attach", false);
    }
}
