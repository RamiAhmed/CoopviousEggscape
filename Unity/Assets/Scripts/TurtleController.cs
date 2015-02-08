using UnityEngine;

public class TurtleController : MonoBehaviour
{
    public float movementSpeed = 3f;

    public GameObject targetPlayer
    {
        get;
        set;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (targetPlayer == null)
        {
            return;
        }

        Vector3 direction = (targetPlayer.transform.position - this.transform.position).normalized;
        this.transform.position = this.transform.position + direction * Time.fixedDeltaTime * movementSpeed;
    }
}