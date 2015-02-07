using UnityEngine;

namespace Exploder.Demo
{
    public class ThrowObject : MonoBehaviour
    {
        private float destroyTimer;

        private void Start()
        {
            destroyTimer = 10.0f;
        }

        private void Update()
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer < 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
