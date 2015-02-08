using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        PLAY
    }

    public Canvas UI;
    public AudioClip mainMusic;

    private AudioSource _audioPlayer;

    private GameState _gameState = GameState.MENU;

    private void Start()
    {
        _audioPlayer = this.GetComponent<AudioSource>();
        if (_audioPlayer == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its AudioSource for playing the main music");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameState = GameState.MENU;
            UI.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        UI.gameObject.SetActive(false);
        _gameState = GameState.PLAY;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}