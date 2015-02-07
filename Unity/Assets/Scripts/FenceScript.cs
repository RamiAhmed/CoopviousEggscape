using UnityEngine;
using System.Collections;

public class FenceScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Egg")
		{
			collision.gameObject.GetComponent<EggController>().Explode();
			rigidbody.isKinematic = false;
			Destroy(gameObject, 1f);
		}
	}

}
