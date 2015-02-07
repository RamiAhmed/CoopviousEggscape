using UnityEngine;
using System.Collections;

public class DoubleTriggerDoorRemover : DoorTriggerBase
{
    public GameObject triggerPartner;
    public float activationTime = 3f;

    private float _lastActivation;
    private bool _activated;

    public bool activated
    {
        get
        {
            return _activated;
        }
    }

    private void Update()
    {
        if (!_activated)
        {
            return;
        }

        float currentTime = Time.time;
        if (currentTime - _lastActivation > activationTime)
        {
            _activated = false;
            this.renderer.material.color = Color.yellow; // TODO: REMOVE THIS DEBUG
        }
    }

    protected override void HandleDoor(Collider other, GameObject door)
    {
        if (!_activated)
        {
            var partner = triggerPartner.GetComponent<DoubleTriggerDoorRemover>();
            if (partner == null)
            {
                Debug.LogError(this.gameObject.name + " could not find the partner trigger!");
            }

            this.renderer.material.color = Color.green; // TODO: REMOVE THIS DEBUG
            _activated = true;
            _lastActivation = Time.time;

            if (partner.activated)
            {
                GameObject.Destroy(door, 0.10f); // door is removed
                GameObject.Destroy(triggerPartner, 0.125f); // destroy partner trigger
                GameObject.Destroy(this.gameObject, 0.15f); // trigger removes itself
            }
        }
    }
}
