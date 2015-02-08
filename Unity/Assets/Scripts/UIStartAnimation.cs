using Exploder;
using UnityEngine;

public class UIStartAnimation : MonoBehaviour
{
    public float Speed = 2.3f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private Quaternion _rotation;
    private ExploderObject _exploder;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = rigidbody;
        _rotation = transform.rotation;
        _exploder = GetComponent<ExploderObject>();

        Invoke("Phase1", 0f);
        Invoke("Phase2", 2.4f);
        Invoke("Phase3", 3.3f);
        Invoke("Phase4", 4.2f);
        Invoke("Phase5", 5f);
    }

    private void Phase1()
    {
        _rigidbody.velocity = new Vector3(Speed * -1f, 0, 0);
        _animator.SetBool("Walking", true);
    }

    private void Phase2()
    {
        _rigidbody.velocity = Vector3.zero;
        _animator.SetBool("Walking", false);
        transform.eulerAngles = new Vector3(-85f, 180f, 0);
        _animator.SetBool("Hit", true);
    }

    private void Phase3()
    {
        transform.eulerAngles = new Vector3(0f, 90f, 90f);
        _rigidbody.velocity = new Vector3(Speed, 0, 0);
        _animator.SetBool("Walking", true);
    }

    private void Phase4()
    {
        transform.eulerAngles = new Vector3(0f, 270f, -85f);
        _rigidbody.velocity = new Vector3(Speed * -1f, 0, 0);
    }

    private void Phase5()
    {
        _rigidbody.velocity = Vector3.zero;
        _animator.SetBool("Walking", false);
        transform.eulerAngles = new Vector3(-85f, 180f, 0);
        _exploder.Explode();
    }
}