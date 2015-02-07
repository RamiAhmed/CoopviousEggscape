using UnityEngine;

namespace Exploder.Demo
{
    /// <summary>
    /// this demo script demonstrates example how to explode object that have Exploder as a component
    /// </summary>
    public class DemoSelfExplode : MonoBehaviour
    {
        public Camera Camera;

        private void Start()
        {
            Application.targetFrameRate = 60;

            if (!Camera)
            {
                Camera = UnityEngine.Camera.main;
            }
        }

        private bool IsExplodable(GameObject obj)
        {
            return obj.CompareTag(ExploderObject.Tag);
        }

        private void Update()
        {
            // we hit the mouse button
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Ray mouseRay;

                if (Camera)
                {
                    mouseRay = Camera.ScreenPointToRay(Input.mousePosition);
                }
                else
                {
                    mouseRay = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                }

                RaycastHit hitInfo;

                // we hit the object
                if (Physics.Raycast(mouseRay, out hitInfo))
                {
                    var obj = hitInfo.collider.gameObject;

                    // explode this object!
                    if (IsExplodable(obj))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            ExplodeObject(obj);
                        }
                    }
                }
            }
        }

        private void ExplodeObject(GameObject obj)
        {
            // get exploder
            var ex = obj.GetComponent<ExploderObject>();

            if (ex)
            {
                ex.Explode(OnExplosion);
            }
        }

        private void OnExplosion(float time, ExploderObject.ExplosionState state)
        {
            if (state == ExploderObject.ExplosionState.ExplosionFinished)
            {
                //UnityEngine.Debug.Log("Exploded");
            }
        }
    }
}
