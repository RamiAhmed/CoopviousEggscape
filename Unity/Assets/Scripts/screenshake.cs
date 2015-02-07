using UnityEngine;
using System.Collections;

public class screenshake : MonoBehaviour {

	public bool shaking = false;
	public float angle = 0.8f;
	public float speed = 0.05f;

	void Start () {
	}

//	void Update () {
//	}
	public void ShakeNow(int duration)
	{
		StartCoroutine(shakeItBaby(duration));
	}

	public IEnumerator shakeItBaby (int duration)
	{
		if (!shaking) {
			shaking = true;
			print ("Shake!");

			transform.Rotate (0, 0, -angle);
			for (int i = 1; i <= duration; i++) {
				transform.Rotate (0, 0, 2*angle);
				//print ("Shake left");
				yield return new WaitForSeconds (speed);
				transform.Rotate (0, 0, -2*angle);
				//print ("Shake right");
				yield return new WaitForSeconds (speed);

			}
			transform.Rotate (0, 0, angle);
			shaking = false;
			yield break;
		} else {
			//print ("Already shaking!");
			yield break;
		}
	}
	
}
