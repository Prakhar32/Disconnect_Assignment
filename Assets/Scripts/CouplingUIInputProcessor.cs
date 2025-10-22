using UnityEngine;
using TMPro;

public class CouplingUIInputProcessor : MonoBehaviour
{
    public GameObject Coupling;
    public TMP_Dropdown Dropdown;

    private GroupTransparencySetter _groupTransparencySetter;
    private CouplingStateMachine _couplingStateMachine;

    void Start()
    {
        if(Coupling == null)
        {
            Destroy(this);
            throw new MissingReferenceException("Coupling is missing");
        }

        _groupTransparencySetter = Coupling.GetComponent<GroupTransparencySetter>();
        if(_groupTransparencySetter == null)
        {
            Destroy(this);
            throw new MissingComponentException("GroupTransparencySetter component is missing from Coupling");
        }

        _couplingStateMachine = Coupling.GetComponent<CouplingStateMachine>();
        if(_couplingStateMachine == null)
        {
            Destroy(this);
            throw new MissingComponentException("CouplingStateMachine component is missing from Coupling");
        }

        if(Dropdown == null)
        {
            Destroy(this);
            throw new MissingReferenceException("Dropdown is missing");
        }
    }

    public void Attach()
    {
        _couplingStateMachine.Attach();
    }

    public void Detach()
    {
        _couplingStateMachine.Detach();
    }

    public void TransparencyChanged()
    {
        int option = Dropdown.value;
        if (option == 0)
            _groupTransparencySetter.MakeOpaque();
        else
            _groupTransparencySetter.MakeTransparent();
    }
}
