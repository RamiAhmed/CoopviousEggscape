using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        PLAY
    }

    public Canvas UI;

    private FadeToBlack _fader;

    private AudioSource _audioPlayer;

    private GameState _gameState = GameState.MENU;

    public GameState gameState
    {
        get { return _gameState; }
    }

    private void Start()
    {
        _fader = this.GetComponent<FadeToBlack>();
        if (_fader == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its FadeToBlack component for fading the screen to black");
        }

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

    public void FadeToBlack(float fadeInTime, float fadeOutTime)
    {
        _fader.StartFade(fadeOutTime, fadeInTime, Color.black);
    }

    public void LoseGame()
    {
        Invoke("Restart", 1f);
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}