using UnityEngine;

public class ActivateDoorTrigger : DoorTriggerBase
{
    protected override void HandleDoor(Collider other, PortalDoor door)
    {
        // activate the portal door
        door.ActivateDoor();

        // tell the egg to explode
        other.GetComponent<EggController>().Explode();

        GameObject.Destroy(this.gameObject, 0.15f); // trigger removes itself
    }
}