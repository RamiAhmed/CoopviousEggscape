using UnityEngine;

public class InputReceiver : MonoBehaviour
{
    private PlayerController _playerController;

    // Use this for initialization
    private void Start()
    {
        _playerController = this.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_playerController == null)
        {
            return;
        }

        float horizontalMovement = Input.GetAxis("Horizontal " + _playerController.playerNumber);
        float verticalMovement = Input.GetAxis("Vertical " + _playerController.playerNumber);
        if (Mathf.Abs(horizontalMovement) > 0.2f || Mathf.Abs(verticalMovement) > 0.2f)
        {
            _playerController.Move(horizontalMovement, verticalMovement);
        }

        bool attacked = Input.GetButton("Punch " + _playerController.playerNumber);
        if (attacked)
        {
            _playerController.Attack();
        }

        float rotateX = Input.GetAxis("Rotate X " + _playerController.playerNumber);
        float rotateY = Input.GetAxis("Rotate Y " + _playerController.playerNumber);
        if (Mathf.Abs(rotateX) > 0.2f || Mathf.Abs(rotateY) > 0.2f)
        {
            _playerController.Rotate(rotateX, rotateY);
        }
    }
}