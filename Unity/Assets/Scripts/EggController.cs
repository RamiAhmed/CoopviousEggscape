using UnityEngine;
using System.Collections;

public class EggController : MonoBehaviour {

    public float eggMoveSpeed = 10f;
    public float eggMaxLifespan = 10f;

    private float _eggLifespan;
    private bool _destroyed;

    public Vector3 direction
    {
        get;
        set;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
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
    }
}
