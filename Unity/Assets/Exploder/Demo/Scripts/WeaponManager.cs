using UnityEngine;

namespace Exploder.Demo
{
    public class WeaponManager : MonoBehaviour
    {
        public GameObject Shotgun;
        public GameObject RPG;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ExploderUtils.SetActiveRecursively(RPG, false);
                ExploderUtils.SetActiveRecursively(Shotgun, true);

                Shotgun.GetComponent<ShotgunController>().OnActivate();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ExploderUtils.SetActiveRecursively(RPG, true);
                ExploderUtils.SetActiveRecursively(Shotgun, false);

                RPG.GetComponent<RPGController>().OnActivate();
            }
        }
    }
}
