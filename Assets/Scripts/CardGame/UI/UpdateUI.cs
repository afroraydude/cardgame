using System;
using CardGame.Management;
using CardGameShared.Data;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{

    public class UpdateUI : MonoBehaviour
    {
        [SerializeField] public PlayManager playManager;

        [SerializeField] public Slider energyBarSlider;
        [SerializeField] public Button[] actions;
        [SerializeField] public Sprite[] actionImages;

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
                    case ActionType.NullAction:
                        actions[i].image.sprite = actionImages[4];
                        break;
                    case ActionType.HeavySwordH:
                        actions[i].image.sprite = actionImages[0];
                        break;
                    case ActionType.Shield:
                        actions[i].image.sprite = actionImages[1];
                        break;
                    case ActionType.Sword:
                        actions[i].image.sprite = actionImages[2];
                        break;
                    case ActionType.HeavySwordS:
                        actions[i].image.sprite = actionImages[3];
                        break;
                }

                if (actionType >= ActionType.HeavySwordH)
                {
                    //actions[i].image.sprite = actionImages[actionType];
                }
            }

            //energyPoints.text = playManager.energyPoints.ToString();
            energyBarSlider.value = playManager.energyPoints;
        }
    }
}