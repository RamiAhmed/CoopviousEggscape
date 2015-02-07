using UnityEngine;

public class ActivateDoorTrigger : EventTrigger
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
            var portalDoor = _door.GetComponent<PortalDoor>();
            if (portalDoor == null)
            {
                Debug.LogError(this.gameObject.name + " could not find PortalDoor component on " + _door);
                return;
            }

            portalDoor.ActiveDoor();

            //GameObject.Destroy(_door, 0.1f);
            GameObject.Destroy(other.gameObject, 0.1f);
            GameObject.Destroy(this.gameObject, 0.15f);
        }
    }

    protected override void EndCollision(Collider other)
    {
    }
}