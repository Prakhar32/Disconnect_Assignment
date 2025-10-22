using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CouplingInputTests
{
    [UnityTest]
    public IEnumerator CouplingMissing()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        CouplingUIInputProcessor couplingUI = g.AddComponent<CouplingUIInputProcessor>();
        yield return null;

        Assert.IsTrue(couplingUI == null);
    }

    [UnityTest]
    public IEnumerator GroupTransparencynotPresentinCoupling()
    {
        LogAssert.ignoreFailingMessages = true;
        
        GameObject c = GameObject.Instantiate(Resources.Load<GameObject>(Constants.CouplingPrefabPath));
        Object.Destroy(c.GetComponent<GroupTransparencySetter>());
        yield return null;
        
        GameObject g = new GameObject();
        CouplingUIInputProcessor couplingUI = g.AddComponent<CouplingUIInputProcessor>();
        couplingUI.Coupling = c;
        yield return null;

        Assert.IsTrue(couplingUI == null);
    }

    [UnityTest]
    public IEnumerator CouplingStaateMachine_notPresentinCoupling()
    {
        LogAssert.ignoreFailingMessages = true;

        GameObject c = GameObject.Instantiate(Resources.Load<GameObject>(Constants.CouplingPrefabPath));
        Object.Destroy(c.GetComponent<CouplingStateMachine>());
        yield return null;

        GameObject g = new GameObject();
        CouplingUIInputProcessor couplingUI = g.AddComponent<CouplingUIInputProcessor>();
        couplingUI.Coupling = c;
        yield return null;

        Assert.IsTrue(couplingUI == null);
    }

    [UnityTest]
    public IEnumerator UI_CannotBeNull()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        CouplingUIInputProcessor couplingUI = g.AddComponent<CouplingUIInputProcessor>();
        couplingUI.Coupling = GameObject.Instantiate(Resources.Load<GameObject>(Constants.CouplingPrefabPath));
        yield return null;
        Assert.IsTrue(couplingUI == null);
    }
}
