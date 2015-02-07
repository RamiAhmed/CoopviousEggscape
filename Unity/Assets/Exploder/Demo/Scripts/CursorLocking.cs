using UnityEngine;

namespace Exploder.Demo
{
    public class CursorLocking : MonoBehaviour
    {
        public static bool IsLocked;

        private void Update()
        {
            IsLocked = Screen.lockCursor;

            if (Input.GetMouseButtonDown(0))
            {
                Screen.lockCursor = true;
                Screen.showCursor = false;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Screen.lockCursor = false;
                Screen.showCursor = true;
            }

            if (Screen.lockCursor == false)
            {
                Screen.showCursor = true;
            }
        }
    }
}
