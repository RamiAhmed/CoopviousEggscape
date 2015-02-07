using UnityEngine;

public class OpenDoorTrigger : EventTrigger
{
    private GameObject _door;

    private void Start()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        float shortestDistance = float.MaxValue;
        GameObject nearestDoor = null;
        for (int i = 0; i < doors.Length; i++)
        {
            float distance = (doors[i].transform.position - transform.position).sqrMagnitude;
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestDoor = doors[i];
            }
        }

        _door = nearestDoor;
    }

    protected override void StartCollision(Collider other)
    {
        if (_door != null)
        {
            GameObject.Destroy(_door, 0.1f);
            GameObject.Destroy(other.gameObject, 0.1f);
        }
    }

    protected override void EndCollision(Collider other)
    {
    }
}