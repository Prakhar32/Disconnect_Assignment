using UnityEngine;

public class SingleInstanceEnsurer : MonoBehaviour
{
    void Start()
    {
        if (FindObjectsByType<SingleInstanceEnsurer>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
