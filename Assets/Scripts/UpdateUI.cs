using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{

    public class UpdateUI : MonoBehaviour
    {
        [SerializeField] private Text energyPoints;
        [SerializeField] private Text[] actionTexts;
        [SerializeField] public PlayManager playManager;

        private void Start()
        {
            
        }

        public void UpdateTextElements()
        {
            for (int i = 0; i <= 4; i++)
            {
                int actionType = playManager.actionSet[i];

                switch (actionType)
                {
                    case 0:
                        actionTexts[i].text = "Heavy";
                        break;
                    case 1:
                        actionTexts[i].text = "Shield";
                        break;
                    case 2:
                        actionTexts[i].text = "Sword";
                        break;
                }
            }

            energyPoints.text = playManager.energyPoints.ToString();
        }
    }
}