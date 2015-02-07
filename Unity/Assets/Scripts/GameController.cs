using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public enum GameState {
		MENU,
		PLAY
	}
	public Canvas UI;

	private GameState _gameState = GameState.MENU;

	void Start () {
		
	}

	void Update () {
	
	}

	public void StartGame() {
		UI.gameObject.SetActive(false);
		_gameState = GameState.PLAY;
	}

	public void QuitGame() {
		Application.Quit();
	}
}
