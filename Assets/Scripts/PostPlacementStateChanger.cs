using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PostPlacementStateChanger : MonoBehaviour
{
    private ObjectSpawner _spawner;
    private ARInteractorSpawnTrigger _spawnTrigger;

    public CouplingUIInputProcessor CouplingInteractionUI;
    void Start()
    {
        _spawnTrigger = GetComponent<ARInteractorSpawnTrigger>();
        _spawner = GetComponent<ObjectSpawner>();

        if( _spawnTrigger == null)
        {
            Destroy(this);
            throw new MissingComponentException("ARInteractorSpawnTrigger missing");
        }

        if (_spawner == null)
        {
            Destroy(this);
            throw new MissingComponentException("ObjectSpawner missing");
        }

        if(CouplingInteractionUI == null)
        {
            Destroy(this);
            throw new MissingReferenceException("CouplingInteractionUI reference missing");
        }

        _spawner.objectSpawned += _spawner_objectSpawned;
    }

    private void _spawner_objectSpawned(GameObject obj)
    {
        _spawnTrigger.enabled = false;
        CouplingInteractionUI.gameObject.SetActive(true);
    }
}
