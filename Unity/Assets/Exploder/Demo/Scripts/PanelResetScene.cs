using System.Collections.Generic;
using UnityEngine;

namespace Exploder.Demo
{
    public class PanelResetScene : UseObject
    {
        private List<GameObject> objectList;

        private void Start()
        {
            objectList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Exploder"));
        }

        public override void Use()
        {
            base.Use();

            ExploderUtils.ClearLog();

            foreach (var o in objectList)
            {
                ExploderUtils.SetActiveRecursively(o, true);
                ExploderUtils.SetVisible(o, true);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Use();
            }
        }
    }
}
