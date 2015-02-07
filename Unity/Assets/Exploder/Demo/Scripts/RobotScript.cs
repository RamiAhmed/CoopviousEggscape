using UnityEngine;

namespace Exploder.Demo
{
    internal class RobotScript : MonoBehaviour
    {
        public float radius = 4;
        public float velocity = 1;

        private float angle;
        private Vector3 center;
        private Vector3 lastPos;

        private void Start()
        {
            center = gameObject.transform.position;
        }

        private void Update()
        {
            animation.Play();
        }

        private void FixedUpdate()
        {
            var pos = gameObject.transform.position;

            pos.x = center.x + Mathf.Sin(angle)*radius;
            pos.z = center.z + Mathf.Cos(angle)*radius;

            gameObject.transform.position = pos;

            gameObject.transform.forward = pos - lastPos;
            lastPos = pos;

            angle += Time.deltaTime*velocity;
        }
    }
}
