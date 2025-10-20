using UnityEngine;

public interface CouplingState
{
    void OnEnterState();
    void Attach();
    void Detach();
}
