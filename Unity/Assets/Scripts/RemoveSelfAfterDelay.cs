using UnityEngine;

public class RemoveSelfAfterDelay : MonoBehaviour
{
    public float removalDelay = 5f;

    private float _spawnedTime;
    private bool _destroyed;

    // Use this for initialization
    private void Start()
    {
        _spawnedTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_destroyed)
        {
            return;
        }

        if (Time.time - _spawnedTime > removalDelay)
        {
            _destroyed = true;
            Destroy(this.gameObject, 0.1f);
        }
    }
}