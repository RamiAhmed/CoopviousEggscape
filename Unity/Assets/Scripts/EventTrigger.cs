using UnityEngine;
using System.Collections;

public abstract class EventTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            StartCollision(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Egg"))
        {
            EndCollision(other);
        }
    }

    protected abstract void StartCollision(Collider other);

    protected abstract void EndCollision(Collider other);

}
