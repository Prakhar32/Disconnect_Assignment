using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AnimationTests
{
    [UnityTest]
    public IEnumerator AttachPlays_AttachingAnimation()
    {
        //Given
        GameObject coupling = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        //When
        Animator animator = coupling.GetComponent<Animator>();
        AnimationController controller = coupling.GetComponent<AnimationController>();
        controller.PlayAttachAnimation();
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
        AnimationController controller = coupling.GetComponent<AnimationController>();
        controller.PlayAttachAnimation();
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !animator.IsInTransition(0));
        controller.PlayDetachAnimation();
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
        AnimationController controller = coupling.GetComponent<AnimationController>();
        controller.PlayDetachAnimation();
        yield return new WaitForSeconds(1);

        //Then
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        Object.Destroy(coupling);
    }
}
