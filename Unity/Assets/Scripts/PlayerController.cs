using UnityEngine;

public class PlayerController : SoundPlayerBase
{
    public float maxPlayerSpeed = 20f;
    public float minPlayerSpeed = 1f;
    public float playerAcceleration = 2f;
    public int playerNumber = 1;
    public float dragFactor = 2f;
    public float playerRadius = 2f;
    public float playerAttackConeInDegrees = 45;
    public int maxAttacksPerSecond = 2;
    public float cameraEdgeFactor = 10f;

    public GameObject eggPrefab;
    public GameObject otherPlayer;

	public AudioClip[] attackSoundsImpact;
	public AudioClip[] attackSoundsScream;
	public AudioClip[] attackSoundsMiss;

    private Vector3 _velocity;
    private float _lastAttack;
    private Animator _animator;

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

        _animator = this.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError(this.gameObject.name + " is missing its Animator component");
        }
    }

    private void Update()
    {
        _animator.SetBool("walking", _velocity.sqrMagnitude > minPlayerSpeed);
    }

    private void FixedUpdate()
    {
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

        // TODO: Try to figure out how to vibrate the controller ?
        Debug.Log("Player " + playerNumber + " Attack!");

        Vector3 otherPlayerPos = otherPlayer.transform.position;
        Vector3 selfPos = this.transform.position;

        if ((otherPlayerPos - selfPos).sqrMagnitude < (playerRadius * playerRadius))
        {
            // other player within attack cone radius
            Vector3 eggDirection = (otherPlayerPos - selfPos).normalized;
            MakeEgg(otherPlayerPos, eggDirection);
            PlayRandomSound(attackSoundsImpact);
        }
        else
        {
            PlayRandomSound(attackSoundsMiss);
        }
    }

    private void MakeEgg(Vector3 position, Vector3 direction)
    {
        PlayRandomSound(attackSoundsScream);

        // create new egg
        var newEgg = Instantiate(eggPrefab, position, this.transform.rotation) as GameObject;

        // set egg move direction
        var eggController = newEgg.GetComponent<EggController>();
        eggController.direction = direction;
        
        // TODO: Set other egg properties
    }
}