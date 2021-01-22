﻿using System;
using CardGame.Data;
using CardGame.Networking;
using CardGame.UI;
using CardGameShared.Data;
using CardGameShared.Exception;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MessageType = CardGameShared.Data.MessageType;

namespace CardGame.Management
{
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] public ActionType[] actionSet = new[] {ActionType.NullAction,ActionType.NullAction,ActionType.NullAction,ActionType.NullAction,ActionType.NullAction};
        private WebsocketBehavior _websocket;
        [SerializeField] private Image avatar;
        [SerializeField] private Sprite[] avatars = new Sprite[6];
        internal SaveFile gameSave = new SaveFile();
        internal Player me;
        [SerializeField] private UpdateUI uiUpdater;
        [SerializeField] private Button battleButton;
        [SerializeField] private GameRoundContainer _gameRoundContainer;

        private void Awake()
        {
            _websocket = GetComponent<WebsocketBehavior>();
            _gameRoundContainer = GameObject.Find("GameRound").GetComponent<GameRoundContainer>();

            gameSave = JsonConvert.DeserializeObject<SaveFile>(PlayerPrefs.GetString("save"));
            me = new Player
            {
                actions = actionSet,
                lockedIn = false,
                avatar = gameSave.avatar,
                name = gameSave.name 
            };
            avatar.sprite = avatars[(int) me.avatar];

            int test = (int) MessageType.RoundPlayAccept;
            Debug.Log(test);

        }

        void Start()
        {
        }

        public int energyPoints = 7;

        public bool InsertAction(int spot, ActionType action)
        {
            if (spot == 4 && action == ActionType.HeavySwordH)
            {
                Debug.Log($"err1 {spot} {action}");
                return false;
            }

            if (spot - 1 >= 0 && (actionSet[spot - 1]) == ActionType.HeavySwordH)
            {
                Debug.Log($"err2 {spot} {action}");
                if (action == 0)
                {
                    actionSet[spot + 1] = ActionType.NullAction;
                }

                return false;
            }

            if (!CheckEnergy(spot, action))
            {
                if (action == (int) ActionType.HeavySwordH)
                {
                    actionSet[spot + 1] = ActionType.NullAction;
                }

                return false;
            }

            actionSet[spot] = action;
            return true;
        }

        public bool CheckEnergy(int spot, ActionType action)
        {
            int energy = energyPoints;
            // if spot was previously filled in
            if (actionSet[spot] != ActionType.NullAction)
            {
                // restore points
                switch (actionSet[spot])
                {
                    case ActionType.HeavySwordH:
                        energy += 0;
                        break;
                    case ActionType.Shield:
                        energy += 1;
                        break;
                    case ActionType.Sword:
                        energy += 2;
                        break;
                    case ActionType.HeavySwordS:
                        energy += 2;
                        break;
                }
            }

            switch (actionSet[spot])
            {
                case ActionType.HeavySwordH:
                    energy -= 0;
                    break;
                case ActionType.Shield:
                    energy -= 1;
                    break;
                case ActionType.Sword:
                    energy -= 2;
                    break;
                case ActionType.HeavySwordS:
                    energy -= 2;
                    break;
            }

            if (energy < 0) return false;
            return true;
        }

        public void CalculateEnergy()
        {
            int energy = 7;
            for (int i = 0; i <= 4; i++)
            {
                ActionType actionType = actionSet[i];
                switch (actionType)
                {
                    case ActionType.HeavySwordH:
                        energy -= 0;
                        break;
                    case ActionType.Shield:
                        energy -= 1;
                        break;
                    case ActionType.Sword:
                        energy -= 2;
                        break;
                    case ActionType.HeavySwordS:
                        energy -= 2;
                        break;
                }
            }

            this.energyPoints = energy;
        }

        public void PlayRound()
        {
            CalculateEnergy();
            if (this.energyPoints >= 0)
            {
                ProperMessage play = new ProperMessage
                {
                    messageType = MessageType.RoundPlay,
                    messageData = JsonConvert.SerializeObject(me)
                };
                _websocket.SendSocketMessage(play);
            }
            WaitOnOpponent();
        }

        public void ProcessRoundPlayed(GameRound gameRound)
        {
            Debug.Log("process round");
            
            /*
            SceneManager.LoadScene(4, LoadSceneMode.Additive);
            Debug.Log("process round2");
            BattleSceneManager bsm = GameObject.Find("BattleSceneManager").GetComponent<BattleSceneManager>();
            Debug.Log("process round3");
            bsm.RunBattleScreen(me, gameRound);
            Debug.Log("process round4");
            SceneManager.UnloadSceneAsync("Game");
            */
            
            /*
            string gameRoundPP = JsonConvert.SerializeObject(gameRound);
            Debug.Log("process round2");
            string mePP = JsonConvert.SerializeObject(me);
            Debug.Log("process round3");
            PlayerPrefs.SetString("lg", gameRoundPP);
            Debug.Log("process round4");
            PlayerPrefs.SetString("lastme", mePP);
            Debug.Log("process round5");
            PlayerPrefs.Save();
            Debug.Log("process round6");
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            Debug.Log("process round7");
            */

            _gameRoundContainer.gameRound = gameRound;
            _gameRoundContainer.me = me;
            Debug.Log("process round2");
            _websocket._ws.Close();
            Debug.Log("process round3");
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);
            Debug.Log("process round3");
        }

        public void WaitOnOpponent()
        {
            
        }

        public void ResetActions()
        {
            
            battleButton.enabled = true;
            actionSet = new[] {ActionType.NullAction,ActionType.NullAction,ActionType.NullAction,ActionType.NullAction,ActionType.NullAction};
            uiUpdater.UpdateTextElements();
        }
    }
}