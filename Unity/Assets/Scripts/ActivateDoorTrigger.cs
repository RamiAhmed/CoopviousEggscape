using UnityEngine;

public class ActivateDoorTrigger : DoorTriggerBase
{
    protected override void HandleDoor(Collider other, GameObject door)
    {
        var portalDoor = door.GetComponent<PortalDoor>();
        if (portalDoor == null)
        {
            Debug.LogError(this.gameObject.name + " could not find PortalDoor component on " + _door);
            return;
        }

        // activate the portal door
        portalDoor.ActivateDoor();

        // tell the egg to explode
        other.GetComponent<EggController>().Explode();

        GameObject.Destroy(this.gameObject, 0.15f); // trigger removes itself
    }
}