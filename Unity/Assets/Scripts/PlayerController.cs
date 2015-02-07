using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxPlayerSpeed = 20f;
    public float playerSpeed = 10f; 
    public int playerNumber = 1;
    public float dragFactor = 2f;

    private Vector3 _velocity;

	// Use this for initialization
	void Start () {
        Debug.Log("Player " + playerNumber + " ready!");
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    private void FixedUpdate()
    {
        if (_velocity.sqrMagnitude > 0f)
        {
            Vector3 speed = _velocity * playerSpeed * Time.fixedDeltaTime;
            this.transform.position = transform.position + speed;
            _velocity -= speed * dragFactor;
        }
    }

    public void Move(float deltaX, float deltaY)
    {
        _velocity += new Vector3(deltaX, deltaY, 0f);
        _velocity = Vector3.ClampMagnitude(_velocity, maxPlayerSpeed);
        //this.transform.position = transform.position + new Vector3(deltaX, deltaY, 0f) * playerSpeed;
        //Debug.Log("Player " + playerNumber + " moving with x: " + deltaX + ", y: " + deltaY);
    }

    public void Attack()
    {
        // TODO: Try to figure out how to vibrate the controller ?
        Debug.Log("Player " + playerNumber + " Attack!");
    }
}
