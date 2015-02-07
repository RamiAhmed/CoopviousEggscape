using UnityEngine;
using System.Collections;

public class CameraSizeController : MonoBehaviour {

	GameObject Player1;
	GameObject Player2;

	// Use this for initialization
	void Start () {
		Player1 = GameObject.Find("Player 1");
		Player2 = GameObject.Find("Player 2");
	}
	
	// Update is called once per frame
	void Update () {
		Resize();
	}

	void Resize() {
		//float distance = Vector3.Distance(Player1.transform.position, Player2.transform.position);

		CenterCamera();

		//Debug.Log(distance);
	}

	void CenterCamera() {
		Vector3 position = CalculateMidVector(Player1.transform.position, Player2.transform.position);
		float distance = Vector3.Distance(Player1.transform.position, Player2.transform.position);

		Camera.main.transform.position = new Vector3(position.x, position.y, -10);
		Camera.main.orthographicSize = distance;
	}

	Vector3 CalculateMidVector(Vector3 first, Vector3 second) {
		return Vector3.Lerp(first, second, 0.5f);
	}
}
