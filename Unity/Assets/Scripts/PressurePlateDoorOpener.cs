using UnityEngine;
using System.Collections;

public class PressurePlateDoorOpener : MonoBehaviour
{
    public float movementSpeed = 3f;
    public GameObject associatedDoor;

    private float _startY;
    private float _height;
    private Vector3 _doorVelocity;

    // Use this for initialization
    void Start()
    {
        if (associatedDoor == null)
        {
            Debug.LogError(this.gameObject.name + " could not find associated door");
        }

        _height = associatedDoor.collider.bounds.extents.y * 2f;
        _startY = associatedDoor.transform.position.y;
    }

    void FixedUpdate()
    {
        if (_doorVelocity.sqrMagnitude > 1f)
        {
            Vector3 speed = _doorVelocity * movementSpeed * Time.fixedDeltaTime;
            _doorVelocity -= speed;

            Vector3 mod = associatedDoor.transform.position + speed;
            if (mod.y > _startY)
            {
                mod.y = _startY;
            }

            associatedDoor.transform.position = mod;
        }
        //else if (associatedDoor.transform.position.y > _startY)
        //{
        //    associatedDoor.transform.position = new Vector3(associatedDoor.transform.position.x, _startY, associatedDoor.transform.position.z);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        _doorVelocity = new Vector3(0f, -_height, 0f);
    }

    void OnTriggerExit(Collider other)
    {
        _doorVelocity = new Vector3(0f, _height, 0f);
    }
}
