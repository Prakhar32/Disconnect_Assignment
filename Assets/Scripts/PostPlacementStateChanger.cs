using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PostPlacementStateChanger : MonoBehaviour
{
    private ObjectSpawner _spawner;
    private ARInteractorSpawnTrigger _spawnTrigger;

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

        _spawner.objectSpawned += _spawner_objectSpawned;
    }

    private void _spawner_objectSpawned(GameObject obj)
    {
        _spawnTrigger.enabled = false;
    }
}
