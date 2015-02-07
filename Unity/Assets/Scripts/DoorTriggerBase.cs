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
            var portalDoor = _door.GetComponent<PortalDoor>();
            if (portalDoor == null)
            {
                Debug.LogError(this.gameObject.name + " could not find PortalDoor component on " + _door);
                return;
            }

            HandleDoor(other, portalDoor);
        }
    }

    protected abstract void HandleDoor(Collider other, PortalDoor door);

    protected override void EndCollision(Collider other)
    {
    }
}