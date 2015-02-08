using UnityEngine;

public class ChickenHandController : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = this.transform.root.GetComponent<PlayerController>();
        if (_playerController == null)
        {
            Debug.LogError(this.gameObject.name + " could not find the parent PlayerController");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("ChickenBody"))
        {
            return;
        }

        _playerController.Hit();
    }

    void OnTriggerExit(Collider other)
    {

    }
}