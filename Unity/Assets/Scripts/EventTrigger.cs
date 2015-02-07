using UnityEngine;

public abstract class EventTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            StartCollision(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            EndCollision(other);
        }
    }

    protected abstract void StartCollision(Collider other);

    protected abstract void EndCollision(Collider other);
}