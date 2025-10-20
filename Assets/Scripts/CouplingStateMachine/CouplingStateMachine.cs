using UnityEngine;

public class CouplingStateMachine : MonoBehaviour
{
    private Animator _animator;
    private CouplingState _currentState;
    internal CouplingState _idleState;
    internal CouplingState _attachmentState;
    internal CouplingState _detachmentState;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Destroy(this);
            throw new MissingComponentException("Animator component is required for CouplingStateMachine.");
        }

        setStates();
    }

    private void setStates()
    {
        _idleState = new IdleState(this);
        _attachmentState = new AttachmentState(this, _animator, this);
        _detachmentState = new DetachmentState(this, _animator);
        _currentState = _idleState;
    }

    internal void SetState(CouplingState newState)
    {
        _currentState = newState;
        _currentState.OnEnterState();
    }

    public void Attach()
    {
        _currentState.Attach();
    }

    public void Detach()
    {
        _currentState.Detach();
    }
}
