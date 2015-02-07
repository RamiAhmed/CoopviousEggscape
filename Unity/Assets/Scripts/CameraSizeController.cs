using UnityEngine;
using System.Collections;

public class CameraSizeController : MonoBehaviour {

	public Camera Camera;

	public GameObject Player0;
	public GameObject Player1;

	public float startCameraSize = 7;
	public float minCameraSize = 7;
	public float maxCameraSize = 15;

	// Use this for initialization
	void Start () {
		Camera.orthographicSize = startCameraSize;
	}
	
	// Update is called once per frame
	void Update () {
		CenterCamera();
		Resize();
	}

	void Resize() {
		float distance = Vector3.Distance(Player0.transform.position, Player1.transform.position);
		if (distance <= minCameraSize) {
			Camera.orthographicSize = minCameraSize;
		} else if (distance >= maxCameraSize) {
			Camera.orthographicSize = maxCameraSize;
		} else {
			Camera.orthographicSize = distance;
		}
	}

	void CenterCamera() {
		Vector3 position = CalculateMidVector(Player0.transform.position, Player1.transform.position);

		Camera.transform.position = new Vector3(position.x, position.y, -10);
	}

	Vector3 CalculateMidVector(Vector3 first, Vector3 second) {
		return Vector3.Lerp(first, second, 0.5f);
	}
}
