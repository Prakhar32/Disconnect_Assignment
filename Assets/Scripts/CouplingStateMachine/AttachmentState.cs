using System.Collections;
using UnityEngine;

public class AttachmentState : CouplingState
{
    private CouplingStateMachine _statemachine;
    private MonoBehaviour _mono;
    private Animator _animator;
    private Coroutine _gasFlowRoutine;
    internal AttachmentState(CouplingStateMachine stateMachine, Animator animator, MonoBehaviour mono)
    {
        _statemachine = stateMachine;
        _animator = animator;
        _mono = mono;
    }
    public void OnEnterState()
    {
        _animator.SetBool("Attach", true);
        _gasFlowRoutine = _mono.StartCoroutine(waitForAttachAnimation());
    }

    private IEnumerator waitForAttachAnimation()
    {
        yield return new WaitUntil(() => !_animator.IsInTransition(0));
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        while (true)
        {
            Vector2 offset = _statemachine.GasMaterial.mainTextureOffset;
            offset.y += Time.deltaTime;
            _statemachine.GasMaterial.mainTextureOffset = offset;
            yield return null;
        }
    }

    public void Attach()
    {
    }

    public void Detach()
    {
        _mono.StopCoroutine(_gasFlowRoutine);
        _statemachine.SetState(_statemachine.GetDetachmentState());
    }
}
