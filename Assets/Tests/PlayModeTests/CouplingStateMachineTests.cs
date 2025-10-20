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
    public IEnumerator GasMaterialCannotBeNull()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        g.AddComponent<Animator>();
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
        yield return new WaitUntil(() => checkIfStateExited(animator, "Idle"));

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Attaching"));

        //Cleanup
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
        yield return new WaitUntil(() => checkIfStateExited(animator, "Idle"));
        stateMachine.Detach();
        yield return new WaitUntil(() => checkIfStateExited(animator, "Attaching"));

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Detaching"));
        
        //Cleanup
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

    [UnityTest]
    public IEnumerator GasStartsFlowing_WhenAttached()
    {
        //Given
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;
        
        //When
        Animator animator = coupling.GetComponent<Animator>();
        CouplingStateMachine stateMachine = coupling.GetComponent<CouplingStateMachine>();
        Material gasMaterial = stateMachine.GasMaterial;
        stateMachine.Attach();
        yield return new WaitUntil(() => checkIfStateExited(animator, "Idle") && isCurrentAnimationCompleted(animator));
        yield return null;

        //Then
        yield return checkIfOffsetKeepsChanging(gasMaterial, 2f);

        //Cleanup
        stateMachine.Detach();
        Object.Destroy(coupling);
    }

    [UnityTest]
    public IEnumerator GasStopsFlowing_WhenDetached()
    {
        //Given
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;
        
        //When
        Animator animator = coupling.GetComponent<Animator>();
        CouplingStateMachine stateMachine = coupling.GetComponent<CouplingStateMachine>();
        Material gasMaterial = stateMachine.GasMaterial;

        stateMachine.Attach();
        yield return new WaitUntil(() => checkIfStateExited(animator, "Idle") && isCurrentAnimationCompleted(animator));
        yield return null;
        
        stateMachine.Detach();
        yield return null;

        //Then
        float previousOffset = gasMaterial.mainTextureOffset.y;
        yield return null;
        float currentOffset = gasMaterial.mainTextureOffset.y;
        Assert.AreEqual(previousOffset, currentOffset, "Gas material offset is still changing after detachment.");

        //Cleanup
        Object.Destroy(coupling);
    }

    [UnityTest]
    public IEnumerator CanReAttach_AfterDetachment()
    {
        //Given
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        //When
        Animator animator = coupling.GetComponent<Animator>();
        CouplingStateMachine stateMachine = coupling.GetComponent<CouplingStateMachine>();
        stateMachine.Attach();
        yield return new WaitUntil(() => checkIfStateExited(animator, "Idle") && isCurrentAnimationCompleted(animator));
        yield return null;

        stateMachine.Detach();
        yield return new WaitUntil(() => checkIfStateExited(animator, "Attaching") && isCurrentAnimationCompleted(animator));
        yield return null;

        stateMachine.Attach();
        yield return new WaitUntil(() => checkIfStateExited(animator, "Idle") && isCurrentAnimationCompleted(animator));
        yield return null;

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Attaching"));
        
        //Cleanup
        Object.Destroy(coupling);
    }

    private IEnumerator checkIfOffsetKeepsChanging(Material gasMaterial, float duration)
    {
        float currentTime = 0;
        float previousOffset = gasMaterial.mainTextureOffset.y;
        while (currentTime < duration)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float currentOffset = gasMaterial.mainTextureOffset.y;
            if(currentOffset == previousOffset)
                Assert.Fail("Gas material offset is not changing over time.");
            previousOffset = currentOffset;
        }
    }

    private bool checkIfStateExited(Animator animator, string fromState)
    {
        return !animator.GetCurrentAnimatorStateInfo(0).IsName(fromState) && !animator.IsInTransition(0);
    }

    private bool isCurrentAnimationCompleted(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
    }
}
