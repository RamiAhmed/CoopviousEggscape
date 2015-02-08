using UnityEngine;

public class PlayerController : SoundPlayerBase
{
    public float maxPlayerSpeed = 10f;
    public float minPlayerSpeed = 1f;
    public float playerAcceleration = 2f;
    public int playerNumber = 1;
    public float dragFactor = 2f;
    public float playerRadius = 4f;
    public float maxAttacksPerSecond = 2f;
    public float cameraEdgeFactor = 10f;
    public float disabledControlsTimeOnAttack = 0.4f;
    public int playerStartLives = 3;
    public float chickenHandVisibleTime = 0.4f;

    public GameObject chickenHand;
    public GameObject eggPrefab;
    public GameObject otherPlayer;

    public AudioClip[] attackSoundsImpact;
    public AudioClip[] attackSoundsScream;
    public AudioClip[] attackSoundsMiss;

    private Vector3 _velocity;
    private float _lastAttack;
    private Animator _animator;
    private ParticleSystem _featherParticles;
    private GameController _gameController;

    private float _lastDisabledControls;

    private int _playerLives;

    public int playerLives
    {
        get
        {
            return _playerLives;
        }
        set
        {
            _playerLives = value;
            if (_playerLives < 0)
            {
                _gameController.LoseGame();
            }

            Debug.Log(this.gameObject.name + " lives left: " + _playerLives);
        }
    }

    public bool disabledControls
    {
        get;
        set;
    }

    public Vector3 velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        Debug.Log("Player " + playerNumber + " ready!");

        if (eggPrefab == null)
        {
            // check for and alert if missing egg prefab
            Debug.LogError(this.gameObject.name + " is missing its eggPrefab!");
        }

        if (otherPlayer == null)
        {
            // check for and alert if missing other player reference
            Debug.LogError(this.gameObject.name + " is missing its otherPlayer reference!");
        }

        if (chickenHand == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its chicken hand prefab");
        }

        chickenHand.SetActive(false);

        _animator = this.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its Animator component");
        }

        _featherParticles = this.GetComponentInChildren<ParticleSystem>();
        if (_featherParticles == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its feather particle system");
        }

        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (_gameController == null)
        {
            Debug.LogError(this.gameObject.name + " could not find the GameController game object with its GameController component");
        }

        _playerLives = playerStartLives;
    }

    private void Update()
    {
        if (_gameController.gameState == GameController.GameState.MENU)
        {
            return;
        }

        _animator.SetBool("Walking", _velocity.sqrMagnitude > minPlayerSpeed);

        float currentTime = Time.time;
        if (currentTime - _lastDisabledControls > disabledControlsTimeOnAttack)
        {
            disabledControls = false;
        }
    }

    private void FixedUpdate()
    {
        if (_gameController.gameState == GameController.GameState.MENU)
        {
            return;
        }

        if (!this.rigidbody.isKinematic)
        {
            this.rigidbody.velocity = this.rigidbody.angularVelocity = Vector3.zero;
        }

        if (_velocity.sqrMagnitude > minPlayerSpeed)
        {
            // move forward in velocity direction as long as there is a velocity
            Vector3 selfPos = this.transform.position;
            Vector3 speed = _velocity * playerAcceleration * Time.fixedDeltaTime;

            Vector3 projectedPos = Camera.main.WorldToScreenPoint(selfPos + speed);
            if (projectedPos.x < cameraEdgeFactor || projectedPos.x > Screen.width - cameraEdgeFactor ||
                projectedPos.y < cameraEdgeFactor || projectedPos.y > Screen.height - cameraEdgeFactor)
            {
                return;
            }

            _velocity -= speed * dragFactor;
            this.rigidbody.MovePosition(selfPos + speed);
            this.transform.LookAt(selfPos + speed);
        }
    }

    public void Move(float deltaX, float deltaY)
    {
        if (disabledControls)
        {
            return;
        }

        // add up velocity gradually
        _velocity += new Vector3(deltaX, 0f, deltaY);

        // make sure velocity stays below max speed
        _velocity = Vector3.ClampMagnitude(_velocity, maxPlayerSpeed);
    }

    public void Attack()
    {
        // check if attacking is allowed
        float currentTime = Time.time;
        if ((currentTime - _lastAttack) < (1f / (float)maxAttacksPerSecond))
        {
            return;
        }

        _lastAttack = currentTime;

        chickenHand.SetActive(true);
        Invoke("HideChickenHand", chickenHandVisibleTime);

        // TODO: Try to figure out how to vibrate the controller ?
        Debug.Log("Player " + playerNumber + " Attack!");

        Vector3 otherPlayerPos = otherPlayer.transform.position;
        Vector3 selfPos = this.transform.position;

        if ((otherPlayerPos - selfPos).sqrMagnitude < (playerRadius * playerRadius))
        {
            // other player within attack cone radius
            Vector3 eggDirection = (otherPlayerPos - selfPos).normalized;

            var otherPlayerController = otherPlayer.GetComponent<PlayerController>();
            otherPlayerController.MakeEgg(otherPlayerPos, eggDirection);
            PlayRandomSound(attackSoundsImpact);
        }
        else
        {
            PlayRandomSound(attackSoundsMiss);
        }
    }

    private void HideChickenHand()
    {
        chickenHand.SetActive(false);
    }

    public void MakeEgg(Vector3 position, Vector3 direction)
    {
        PlayRandomSound(attackSoundsScream);

        // create new egg
        var newEgg = Instantiate(eggPrefab, position, this.transform.rotation) as GameObject;

        // set egg move direction
        var eggController = newEgg.GetComponent<EggController>();
        eggController.direction = direction;

        this.transform.LookAt(otherPlayer.transform.position);
        _velocity = Vector3.zero;
        disabledControls = true;
        _lastDisabledControls = Time.time;

        _animator.SetTrigger("Hit");

        _featherParticles.Play();
    }

    public void FadeToBlack()
    {
        _gameController.FadeToBlack(0.6f, 0.6f);
    }
}