using System.Collections;
using UnityEngine;

public class DetachmentState : CouplingState
{
    private CouplingStateMachine _statemachine;
    private Animator _animator;
    private MonoBehaviour _mono;

    internal DetachmentState(CouplingStateMachine stateMachine, Animator animator, MonoBehaviour mono)
    {
        _statemachine = stateMachine;
        _animator = animator;
        _mono = mono;
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
        _mono.StartCoroutine(waitForDetachment());
    }

    private IEnumerator waitForDetachment()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        _statemachine.SetState(_statemachine.GetIdleState());
    }
}
