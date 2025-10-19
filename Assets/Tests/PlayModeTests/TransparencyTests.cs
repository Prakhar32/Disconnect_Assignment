using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TransparencyTests
{
    [UnityTest]
    public IEnumerator TransparentMaterialMissing()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        TransparencySetter transparencySetter = g.AddComponent<TransparencySetter>();
        yield return null;

        Assert.IsTrue(transparencySetter == null);
    }

    [UnityTest]
    public IEnumerator MeshRendererMissing()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        TransparencySetter transparencySetter = g.AddComponent<TransparencySetter>();
        transparencySetter.TransparentMaterial = new Material(Shader.Find("Standard"));
        yield return null;
        Assert.IsTrue(transparencySetter == null);
    }

    [UnityTest]
    public IEnumerator DependencyInitialised()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        TransparencySetter transparencySetter = g.AddComponent<TransparencySetter>();
        transparencySetter.TransparentMaterial = new Material(Shader.Find("Standard"));
        transparencySetter.Renderer = g.AddComponent<MeshRenderer>();
        yield return null;

        Assert.IsTrue(transparencySetter != null);
    }
}
