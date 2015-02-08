using UnityEngine;

public class TurtleController : MonoBehaviour
{
    public float movementSpeed = 2f;

    public GameObject targetPlayer
    {
        get;
        set;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        if (targetPlayer == null)
        {
            return;
        }

        Vector3 selfPos = this.transform.position;
        Vector3 direction = (targetPlayer.transform.position - selfPos).normalized;
        this.transform.position = selfPos + direction * Time.fixedDeltaTime * movementSpeed;
        this.transform.LookAt(selfPos + direction);
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherRoot = other.transform.root;

        if (!otherRoot.CompareTag("Player"))
        {
            return;
        }

        var playerController = otherRoot.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError(this.gameObject.name + " OnCollisionEnter - colliding player does not have a PlayerController component");
            return;
        }

        Die();
        playerController.playerLives -= 1;
    }
}