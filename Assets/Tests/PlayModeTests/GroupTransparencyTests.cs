using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TestTools;

public class GroupTransparencyTests
{
    [UnityTest]
    public IEnumerator TransparencyListAbsent()
    {
        LogAssert.ignoreFailingMessages = true;
        GameObject g = new GameObject();
        GroupTransparencySetter groupTransparencySetter = g.AddComponent<GroupTransparencySetter>();
        yield return null;
        
        Assert.IsTrue(groupTransparencySetter == null);
    }

    [UnityTest]
    public IEnumerator CanMakeObjectsinListTransparent()
    {
        GameObject g = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        GroupTransparencySetter groupTransparencySetter = g.GetComponent<GroupTransparencySetter>();
        groupTransparencySetter.MakeTransparent();
        yield return null;

        Assert.IsTrue(checkIfMaterialIsCommon(groupTransparencySetter.TransparencySetters, 
            Resources.Load<Material>(Constants.TransparentMaterialPath)));
    }

    [UnityTest]
    public IEnumerator CanMakeMaterialsOpaqueAgain()
    {
        //Given
        GameObject g = GameObject.Instantiate(Resources.Load<GameObject>("Coupling"));
        yield return null;

        GroupTransparencySetter groupTransparencySetter = g.GetComponent<GroupTransparencySetter>();
        Material originalMaterial = Resources.Load<Material>(Constants.OpaqueMaterialPath);

        //When
        groupTransparencySetter.MakeTransparent();
        yield return null;
        groupTransparencySetter.MakeOpaque();
        yield return null;

        Assert.IsTrue(checkIfMaterialIsCommon(groupTransparencySetter.TransparencySetters, originalMaterial));
    }

    private bool checkIfMaterialIsCommon(List<TransparencySetter> setters, Material searchMaterial)
    {
        foreach (var setter in setters)
        {
            List<Material> materials = new List<Material>(setter.GetComponent<MeshRenderer>().materials);
            if(!checkIfMaterialFoundInList(materials, searchMaterial))
                return false;
        }

        return true;
    }

    private bool checkIfMaterialFoundInList(List<Material> materials, Material searchMaterial)
    {
        foreach(Material material in materials) 
        { 
           if (material.name.Contains(searchMaterial.name))
                return true;
        }
        return false;
    }
}
