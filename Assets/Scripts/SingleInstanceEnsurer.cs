using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SingleInstanceEnsurer : MonoBehaviour
{
    private ObjectSpawner _spawner;
    void Start()
    {
        _spawner = GetComponent<ObjectSpawner>();

        if (_spawner == null)
        {
            Destroy(this);
            throw new MissingComponentException("ObjectSpawner missing");
        }

        _spawner.objectSpawned += _spawner_objectSpawned;
    }

    private void _spawner_objectSpawned(GameObject obj)
    {
        _spawner.enabled = false;
    }
}
