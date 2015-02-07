using UnityEngine;

public class RollingForwardLooping : MonoBehaviour
{
    public float movementSpeed = 3f;
    public GameObject startWaypoint;

    // Use this for initialization
    private void Start()
    {
        if (startWaypoint == null)
        {
            Debug.LogError(this.gameObject.name + " could not find the startWaypoint");
        }
    }

    private void FixedUpdate()
    {
        this.transform.position = transform.position + transform.forward * movementSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.CompareTag("Wall"))
        {
            this.transform.position = startWaypoint.transform.position;
        }
    }
}