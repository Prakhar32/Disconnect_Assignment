using UnityEngine;

public class CouplingStateMachine : MonoBehaviour
{
    public Material GasMaterial;

    private Animator _animator;
    
    private CouplingState _currentState;
    private CouplingState _idleState;
    private CouplingState _attachmentState;
    private CouplingState _detachmentState;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Destroy(this);
            throw new MissingComponentException("Animator component is required for CouplingStateMachine.");
        }

        if(GasMaterial == null)
        {
            Destroy(this);
            throw new MissingReferenceException("Gas Material cannot be null");
        }

        setStates();
    }

    private void setStates()
    {
        _idleState = new IdleState(this);
        _attachmentState = new AttachmentState(this, _animator, this);
        _detachmentState = new DetachmentState(this, _animator, this);
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

    internal CouplingState GetIdleState() { return _idleState; }
    internal CouplingState GetAttachmentState() { return _attachmentState; }
    internal CouplingState GetDetachmentState() { return _detachmentState; }

    private void OnDestroy()
    {
        GasMaterial.mainTextureOffset = Vector2.one;
    }
}
