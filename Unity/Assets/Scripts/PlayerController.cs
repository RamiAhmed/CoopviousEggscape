using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float playerSpeed = 0.3f; 
    public int playerNumber = 1;

	// Use this for initialization
	void Start () {
        Debug.Log("Player " + playerNumber + " ready!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(float deltaX, float deltaY)
    {
        this.transform.position = transform.position + new Vector3(deltaX, deltaY, 0f) * playerSpeed;
        //Debug.Log("Player " + playerNumber + " moving with x: " + deltaX + ", y: " + deltaY);
    }

    public void Attack()
    {
        Debug.Log("Player " + playerNumber + " Attack!");
    }
}
