using Exploder;
using UnityEngine;

public class EggController : SoundPlayerBase
{
    public float eggMoveSpeed = 10f;
    public float eggMaxLifespan = 5f;
    public float chanceForEggLeftovers = 0.25f;

    public AudioClip[] eggImpactSoundsDry;
    public AudioClip[] eggImpactSoundsWet;

    public GameObject eggLeftoversGO;

    private float _eggLifespan;
    private bool _destroyed;

    private ExploderObject _exploder;

    public Vector3 direction
    {
        get;
        set;
    }

    protected override void Start()
    {
        base.Start();

        _exploder = this.GetComponentInChildren<ExploderObject>();

        _audioPlayer = this.GetComponent<AudioSource>();
        if (_audioPlayer == null)
        {
            Debug.LogError(this.gameObject.name + " could not find its audio player");
        }

        if (eggLeftoversGO == null)
        {
            Debug.LogWarning(this.gameObject.name + " is missing a eggLeftoversPrefab reference");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        _eggLifespan += Time.deltaTime;

        if (_eggLifespan > eggMaxLifespan)
        {
            Explode(true);
        }
    }

    private void FixedUpdate()
    {
        if (_eggLifespan < eggMaxLifespan && !_destroyed)
        {
            this.transform.position = transform.position + direction * eggMoveSpeed * Time.fixedDeltaTime;
        }

        this.transform.rotation = Quaternion.Lerp(transform.rotation, Random.rotation, Time.fixedDeltaTime * 5f);
    }

    public void Explode(bool bPlayWetSounds = false)
    {
        if (!_destroyed)
        {
            _destroyed = true;

            if (bPlayWetSounds)
            {
                PlayRandomSound(eggImpactSoundsWet, true);
            }
            else
            {
                PlayRandomSound(eggImpactSoundsDry, true);
            }

            this.tag = "Exploder";
            _exploder.Explode();

            if (eggLeftoversGO != null && Random.value < chanceForEggLeftovers)
            {
                Vector3 pos = this.transform.position + new Vector3(0f, 0.5f, 0f);
                Instantiate(eggLeftoversGO, pos, Quaternion.identity);
            }
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.CompareTag("Wall") || coll.transform.CompareTag("Destructible"))
        {
            Explode();

            if (coll.transform.CompareTag("Destructible"))
            {
                coll.rigidbody.isKinematic = false;
                Destroy(coll.gameObject, 3f);
            }
        }
        else if (coll.transform.CompareTag("Turtle") || coll.transform.root.CompareTag("Turtle"))
        {
            Explode();

            var turtleController = coll.transform.GetComponent<TurtleController>();
            if (turtleController == null)
            {
                Debug.LogError(this.gameObject.name + " could not find the TurtleController component on the colliding object: " + coll);
                return;
            }

            turtleController.Die();
        }
    }
}