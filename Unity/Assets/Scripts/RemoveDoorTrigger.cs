using UnityEngine;

public class RemoveDoorTrigger : DoorTriggerBase
{
    protected override void HandleDoor(Collider other, PortalDoor door)
    {
        GameObject.Destroy(door.gameObject, 0.10f); // door is removed
        GameObject.Destroy(this.gameObject, 0.15f); // trigger removes itself
    }
}