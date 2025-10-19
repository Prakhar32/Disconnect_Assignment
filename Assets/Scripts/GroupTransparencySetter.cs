using UnityEngine;

public class GroupTransparencySetter : MonoBehaviour
{
    public Material TransparencyMaterial;
    public Material OpaqueMaterial;
    

    private void Start()
    {
        if (TransparencyMaterial == null)
        {
            Destroy(this);
            throw new MissingReferenceException("TransparencySetter requires a TransparencyMaterial to function.");
        }

        if(OpaqueMaterial == null)
        {
            Destroy(this);
            throw new MissingReferenceException("TransparencySetter requires an OpaqueMaterial to function.");
        }
    }

    public void MakeTransparent()
    {
        
    }
}
