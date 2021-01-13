using System;
using CardGame.Data;
using CardGame.Networking;
using CardGameShared.Data;
using CardGameShared.Exception;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CardGame.Management
{
    public class PlayManager : MonoBehaviour
    {
        [SerializeField] public ActionType[] actionSet = new[] {ActionType.NullAction,ActionType.NullAction,ActionType.NullAction,ActionType.NullAction,ActionType.NullAction};
        [SerializeField] public GameManager gameManager;
        private WebsocketBehavior _websocket;
        [SerializeField] private Image avatar;
        [SerializeField] private Sprite[] avatars = new Sprite[6];
        internal SaveFile gameSave = new SaveFile();
        internal Player me;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _websocket = GetComponent<WebsocketBehavior>();
            if (!gameManager)
            {
                throw new CardGameException(ErrorCode.GameManagerNotLoaded);
            }

            gameSave = gameManager.LoadSaveFile();
            me = new Player
            {
                actions = actionSet,
                lockedIn = false,
                avatar = gameSave.avatar,
                name = gameSave.name 
            };
            avatar.sprite = avatars[(int) me.avatar];
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
        }

        public void ProcessRoundPlayed(GameRound gameRound)
        {
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
            BattleSceneManager bsm = GameObject.Find("BattleSceneManager").GetComponent<BattleSceneManager>();
            bsm.RunBattleScreen(me, gameRound);
            SceneManager.UnloadSceneAsync("Game");
        }
    }
}