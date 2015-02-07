using UnityEngine;

public class EggController : MonoBehaviour
{
    public float eggMoveSpeed = 10f;
    public float eggMaxLifespan = 10f;

    private float _eggLifespan;
    private bool _destroyed;

    public Vector3 direction
    {
        get;
        set;
    }

    // Update is called once per frame
    private void Update()
    {
        _eggLifespan += Time.deltaTime;

        if (_eggLifespan > eggMaxLifespan && !_destroyed)
        {
            Destroy(this.gameObject, 0.01f);
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
}