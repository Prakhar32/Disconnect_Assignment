using UnityEngine;

public class AttachmentState : CouplingState
{
    private CouplingStateMachine _statemachine;
    private MonoBehaviour _mono;
    private Animator _animator;
    internal AttachmentState(CouplingStateMachine stateMachine, Animator animator, MonoBehaviour mono)
    {
        _statemachine = stateMachine;
        _animator = animator;
        _mono = mono;
    }
    public void OnEnterState()
    {
        _animator.SetBool("Attach", true);
    }

    public void Attach()
    {
    }

    public void Detach()
    {
        _statemachine.SetState(_statemachine._detachmentState);
    }
}
