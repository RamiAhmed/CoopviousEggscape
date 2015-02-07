using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class InputReceiver : MonoBehaviour {

    private PlayerController _playerController;
    private GamePadState _padState;
    private GamePadState _prevState;
    private PlayerIndex _playerIndex;

	// Use this for initialization
	void Start () 
    {
        _playerController = this.GetComponent<PlayerController>();
        _playerIndex = (PlayerIndex)_playerController.playerNumber;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_playerController == null)
        {
            return;
        }

        
        GamePadState testState = GamePad.GetState(_playerIndex);
        if (testState.IsConnected)
        {
            Debug.Log("Loaded GamePad " + _playerController.playerNumber);
            _prevState = testState;
            _padState = GamePad.GetState(_playerIndex);
        }

        //float horizontalMovement = Input.GetAxis("Horizontal " + _playerController.playerNumber);
        //float verticalMovement = Input.GetAxis("Vertical " + _playerController.playerNumber);
        float horizontalMovement = _padState.ThumbSticks.Left.X;
        float verticalMovement = _padState.ThumbSticks.Left.Y;

        if (Mathf.Abs(horizontalMovement) > 0.2f || Mathf.Abs(verticalMovement) > 0.2f)
        {
            _playerController.Move(horizontalMovement, verticalMovement);
        }

        //bool punched = Input.GetButton("Fire1 " + _playerController.playerNumber);
        bool attacked = _prevState.Buttons.A == ButtonState.Released && _padState.Buttons.A == ButtonState.Pressed;
        if (attacked)
        {
            _playerController.Attack();
            GamePad.SetVibration(_playerIndex, _padState.Triggers.Left, _padState.Triggers.Right);
        }
	}
}
