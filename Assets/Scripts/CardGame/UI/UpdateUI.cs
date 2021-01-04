using System;
using CardGame.Management;
using CardGameShared.Data;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{

    public class UpdateUI : MonoBehaviour
    {
        [SerializeField] private Text energyPoints;
        [SerializeField] private Text[] actionTexts;
        [SerializeField] public PlayManager playManager;

        [SerializeField] public Slider energyBarSlider;
        //[SerializeField] public Button[] actions;
        //[SerializeField] public Sprite[] actionImages;

        private void Start()
        {
            
        }

        public void UpdateTextElements()
        {
            for (int i = 0; i <= 4; i++)
            {
                ActionType actionType = playManager.actionSet[i];

                switch (actionType)
                {
                    case ActionType.HeavySwordH:
                        actionTexts[i].text = "Heavy";
                        break;
                    case ActionType.Shield:
                        actionTexts[i].text = "Shield";
                        break;
                    case ActionType.Sword:
                        actionTexts[i].text = "Sword";
                        break;
                    case ActionType.HeavySwordS:
                        actionTexts[i].text = "Sword";
                        break;
                }

                if (actionType >= ActionType.HeavySwordH)
                {
                    //actions[i].image.sprite = actionImages[actionType];
                }
            }

            energyPoints.text = playManager.energyPoints.ToString();
            //energyBarSlider.value = playManager.energyPoints;
        }
    }
}