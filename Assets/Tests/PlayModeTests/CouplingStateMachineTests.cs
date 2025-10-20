using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CouplingStateMachineTests
{
    [UnityTest]
    public IEnumerator AnimatorCannotbeMising()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        CouplingStateMachine couplingStateMachine = g.AddComponent<CouplingStateMachine>();
        yield return null;

        Assert.IsTrue(couplingStateMachine == null);
    }

    [UnityTest]
    public IEnumerator AttachPlays_AttachingAnimation()
    {
        //Given
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        //When
        Animator animator = coupling.GetComponent<Animator>();
        CouplingStateMachine stateMachine = coupling.GetComponent<CouplingStateMachine>();
        stateMachine.Attach();
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.IsInTransition(0));

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Attaching"));
        Object.Destroy(coupling);
    }

    [UnityTest]
    public IEnumerator DetachPlays_DetachingAnimation()
    {
        //Given
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        //When
        Animator animator = coupling.GetComponent<Animator>();
        CouplingStateMachine stateMachine = coupling.GetComponent<CouplingStateMachine>();
        stateMachine.Attach();
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.IsInTransition(0));
        stateMachine.Detach();
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Attaching") && !animator.IsInTransition(0));

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Detaching"));
        Object.Destroy(coupling);
    }

    [UnityTest]
    public IEnumerator NoDetachment_ifNoAttachment()
    {
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        //When
        Animator animator = coupling.GetComponent<Animator>();
        CouplingStateMachine stateMachine = coupling.GetComponent<CouplingStateMachine>();
        stateMachine.Detach();
        yield return new WaitForSeconds(1);

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        Object.Destroy(coupling);
    }
}
