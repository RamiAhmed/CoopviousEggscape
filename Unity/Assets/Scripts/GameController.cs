using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        PLAY
    }

    public Canvas UI;

    public float gameStateChangeRatePerSecond = 1f;

    private FadeToBlack _fader;

    private AudioSource _audioPlayer;

    private GameState _gameState = GameState.MENU;

    private float _lastGameStateChange;

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
        float currentTime = Time.time;

        if (_gameState == GameState.MENU)
        {
            if (Input.GetButton("StartGame"))
            {
                if (currentTime - _lastGameStateChange > gameStateChangeRatePerSecond)
                {
                    StartGame();
                    _lastGameStateChange = currentTime;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButton("StartGame"))
            {
                if (currentTime - _lastGameStateChange > gameStateChangeRatePerSecond)
                {
                    _gameState = GameState.MENU;
                    UI.gameObject.SetActive(true);

                    _lastGameStateChange = currentTime;
                }
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                Restart();
            }
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