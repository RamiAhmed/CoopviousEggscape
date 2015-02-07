using UnityEngine;

public abstract class DoorTriggerBase : EventTrigger
{
    protected GameObject _door;

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
            HandleDoor(other, _door);
        }
    }

    protected abstract void HandleDoor(Collider other, GameObject door);

    protected override void EndCollision(Collider other)
    {
    }
}