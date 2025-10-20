using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroupTransparencySetter : MonoBehaviour
{
    public List<TransparencySetter> TransparencySetters;

    private void Start()
    {
        if(TransparencySetters == null || TransparencySetters.Count == 0)
        {
            Destroy(this);
            throw new MissingReferenceException("TransparencySetters list is null or empty.");
        }
    }

    public void MakeTransparent()
    {
        foreach (var setter in TransparencySetters)
            setter.MakeTransparent();
    }

    public void MakeOpaque()
    {
        foreach (var setter in TransparencySetters)
            setter.MakeOpaque();
    }
}
