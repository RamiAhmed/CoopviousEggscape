using Exploder;
using UnityEngine;

public class EggController : SoundPlayerBase
{
    public float eggMoveSpeed = 10f;
    public float eggMaxLifespan = 5f;

    public AudioClip[] eggImpactSoundsDry;
    public AudioClip[] eggImpactSoundsWet;

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

    public void Explode(bool bPlayWetSounds=false)
    {
        if (!_destroyed)
        {
            if (bPlayWetSounds)
            {
                PlayRandomSound(eggImpactSoundsWet, true);
            }
            else
            {
                PlayRandomSound(eggImpactSoundsDry, true);
            }

            _destroyed = true;
            this.tag = "Exploder";
            _exploder.Explode();
        }
    }

    void OnCollisionEnter(Collision coll)
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
    }
}