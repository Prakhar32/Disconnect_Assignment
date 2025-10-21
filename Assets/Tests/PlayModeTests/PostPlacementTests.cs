using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PostPlacementTests
{
    [UnityTest]
    public IEnumerator ObjectSpawner_CannotbeNull()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        PostPlacementStateChanger changer = g.AddComponent<PostPlacementStateChanger>();
        yield return null;

        Assert.IsTrue(changer == null);
    }

    [UnityTest]
    public IEnumerator ARInteractorSpawnTrigger_CannotbeNull()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        g.AddComponent<ObjectSpawner>();
        PostPlacementStateChanger changer = g.AddComponent<PostPlacementStateChanger>();
        yield return null;

        Assert.IsTrue(changer == null);
    }

    [UnityTest]
    public IEnumerator CouplingInteractionUI_CannotBeNull()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        g.AddComponent<ObjectSpawner>();
        g.AddComponent<ARInteractorSpawnTrigger>();
        PostPlacementStateChanger changer = g.AddComponent<PostPlacementStateChanger>();
        yield return null;

        Assert.IsTrue(changer == null);
    }
}
