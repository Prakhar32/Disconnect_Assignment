using UnityEngine;

public class TransparencySetter : MonoBehaviour
{
    public Material TransparentMaterial;
    public MeshRenderer Renderer;

    private void Start()
    {
        if(TransparentMaterial == null)
        {
            Destroy(this);
            throw new MissingReferenceException("TransparentMaterial is not assigned.");
        }

        if(Renderer == null)
        {
            Destroy(this);
            throw new MissingReferenceException("MeshRenderer component is missing.");
        }
    }
}
