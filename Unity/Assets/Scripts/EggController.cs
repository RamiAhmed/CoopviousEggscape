using Exploder;
using UnityEngine;

public class EggController : MonoBehaviour
{
    public float eggMoveSpeed = 10f;
    public float eggMaxLifespan = 10f;

    private float _eggLifespan;
    private bool _destroyed;

    private ExploderObject _exploder;

    public Vector3 direction
    {
        get;
        set;
    }

    private void Start()
    {
        _exploder = this.GetComponentInChildren<ExploderObject>();

        //this.rigidbody.AddRelativeForce(new Vector3(0f, heightForce, 0f), ForceMode.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        _eggLifespan += Time.deltaTime;

        if (_eggLifespan > eggMaxLifespan && !_destroyed)
        {
            Explode();
            _destroyed = true;
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

    public void Explode()
    {
        this.tag = "Exploder";
        _exploder.Explode();
    }
}