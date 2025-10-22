using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class GroupTransparencySetter : MonoBehaviour
{
    public List<TransparencySetter> TransparencySetters;
    public List<GameObject> GasObjects;

    private void Start()
    {
        if(!isListValid(TransparencySetters))
        {
            Destroy(this);
            throw new MissingReferenceException("TransparencySetters list is invalid.");
        }

        if(!isListValid(GasObjects))
        {
            Destroy(this);
            throw new MissingReferenceException("GasObjects list is invalid.");
        }
    }

    public void MakeTransparent()
    {
        foreach (var setter in TransparencySetters)
            setter.MakeTransparent();

        foreach (var gasObject in GasObjects)
            gasObject.SetActive(true);
    }

    public void MakeOpaque()
    {
        foreach (var setter in TransparencySetters)
            setter.MakeOpaque();

        foreach (var gasObject in GasObjects)
            gasObject.SetActive(false);
    }

    private bool isListValid<T>(List<T> setters)
    {
        if (setters == null || setters.Count == 0)
            return false;

        foreach (var setter in setters)
            if (setter == null)
                return false;

        return true;
    }
}
