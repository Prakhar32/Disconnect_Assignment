using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Destroy(this);
            throw new System.Exception("No Animator component found on " + gameObject.name);
        }
    }

    public void PlayAttachAnimation()
    {
        _animator.SetBool("Attach", true);
    }

    public void PlayDetachAnimation()
    {
        _animator.SetBool("Attach", false);
    }

}
