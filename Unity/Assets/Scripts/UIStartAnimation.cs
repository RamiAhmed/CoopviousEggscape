using UnityEngine;
using System.Collections;

public class UIStartAnimation : MonoBehaviour {

	public float Speed = -0.4f;

	private Animator _animator;
	private Rigidbody _rigidbody;
	private Quaternion _rotation;

	void Start () {
		_animator = GetComponent<Animator>();
		_rigidbody = rigidbody;
		_rotation = transform.rotation;

		Invoke("Phase1", 0f);
		Invoke("Phase2", 2.4f);
		Invoke("Phase3", 3.3f);
		Invoke("Phase4", 4.2f);
		Invoke("Phase5", 5f);
	}

	void Phase1() {
		_rigidbody.velocity = new Vector3(Speed, 0, 0);
		_animator.SetBool("Walking", true);
	}

	void Phase2() {
		_rigidbody.velocity = Vector3.zero;
		_animator.SetBool("Walking", false);
		transform.eulerAngles = new Vector3(-85f, 180f, 0);
		_animator.SetBool("Hit", true);
	}
	
	void Phase3() {
		transform.eulerAngles = new Vector3(0f, 90f, 90f);
		_rigidbody.velocity = new Vector3(Speed * -1f, 0, 0);
		_animator.SetBool("Walking", true);
	}
	
	void Phase4() {
		transform.eulerAngles = new Vector3(0f, 270f, -85f);
		_rigidbody.velocity = new Vector3(Speed, 0, 0);
	}
	
	void Phase5() {
		_rigidbody.velocity = Vector3.zero;
		_animator.SetBool("Walking", false);
		transform.eulerAngles = new Vector3(-85f, 180f, 0);
	}
}
