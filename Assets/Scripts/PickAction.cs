using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class PickAction : MonoBehaviour
    {
        public PlayManager playManager;
        [SerializeField]
        private CanvasGroup panel;
        [SerializeField]
        private int spot = -1;

        private UpdateUI uiUpdater;

        // Start is called before the first frame update
        void Start()
        {
            panel.alpha = 0;
            panel.interactable = false;
            uiUpdater = GetComponent<UpdateUI>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TogglePanel(int spot)
        {
            panel.alpha = (panel.alpha > 0) ? 0 : 1;
            panel.interactable = !panel.interactable;
            this.spot = spot;
        }

        public void ActionSelect(int choice)
        {
            // heavy sword
            if (choice == 0)
            {
                if (playManager.InsertAction(spot + 1, 2))
                {
                    if (playManager.InsertAction(spot, choice))
                    {
                        TogglePanel(spot);
                    }
                }
            }
            else
            {
                if (playManager.InsertAction(spot, choice))
                {
                    TogglePanel(spot);
                }
            }
            playManager.CalculateEnergy();
            uiUpdater.UpdateTextElements();
        }
    }
}
