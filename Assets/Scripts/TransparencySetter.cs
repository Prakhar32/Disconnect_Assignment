using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TransparencySetter : MonoBehaviour
{
    private Material _transparentMaterial;
    private Material _opaqueMaterial;
    private MeshRenderer _renderer;
    
    private void Start()
    {
        _transparentMaterial = Resources.Load<Material>(Constants.TransparentMaterialPath);
        if (_transparentMaterial == null)
        {
            Destroy(this);
            throw new MissingReferenceException("TransparentMaterial not found.");
        }

        _renderer = GetComponent<MeshRenderer>();
        if (_renderer == null)
        {
            Destroy(this);
            throw new MissingReferenceException("MeshRenderer component is missing.");
        }

        _opaqueMaterial = _renderer.materials[0];
        if(_opaqueMaterial == null)
        {
            Destroy(this);
            throw new MissingReferenceException("MeshRenderer cannot have no material");
        }
    }

    internal void MakeTransparent()
    {
        List<Material> materials = new List<Material>(_renderer.materials);
        materials[0] = _transparentMaterial;
        _renderer.SetMaterials(materials);
    }

    internal void MakeOpaque()
    {
        List<Material> materials = new List<Material>(_renderer.materials);
        materials[0] = _opaqueMaterial;
        _renderer.SetMaterials(materials);
    }
}
