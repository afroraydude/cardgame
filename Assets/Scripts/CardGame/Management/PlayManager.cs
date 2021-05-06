using System;
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
using Avatar = CardGameShared.Data.Avatar;
using MessageType = CardGameShared.Data.MessageType;
using Random = System.Random;

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
        [SerializeField] private GameObject BattleScene;
        [SerializeField] private GameObject GameScene;
        [SerializeField] private bool SinglePlayer = false;
        public GameManager _manager;
        private void Awake()
        {
            _websocket = GetComponent<WebsocketBehavior>();
            _gameRoundContainer = GameObject.Find("GameRound").GetComponent<GameRoundContainer>();
            _manager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameSave = JsonConvert.DeserializeObject<SaveFile>(PlayerPrefs.GetString("save"));
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
            if (SinglePlayer)
            {
                me.actions = new[]
                {
                    ActionType.HeavySwordH, ActionType.HeavySwordS, ActionType.Shield, ActionType.Shield,
                    ActionType.Shield
                };
                PlayRound();
            }
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
                if (action == ActionType.HeavySwordH)
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
            _gameRoundContainer.gameRound = gameRound;
            _gameRoundContainer.me = me;
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);
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
        
        public GameRound GenerateSinglePlayerPlay(Player self)
        {
            if (!VerifyRoundPlay(self)) throw new Exception();
            else
            {
                Player enemy = GenerateEnemyPlay();
                return DetermineRound(self,enemy);
            }
        }

        private Player GenerateEnemyPlay()
        {
            Random x = new Random();
            bool playIsValid = false;
            Player enemy = new Player();
            enemy.name = "Bot";
            enemy.avatar = (Avatar) x.Next(5);
            enemy.actions = new ActionType[5];
            while (!playIsValid)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i > 0 && enemy.actions[i - 1] == ActionType.HeavySwordH)
                        enemy.actions[i] = ActionType.HeavySwordS;
                    else enemy.actions[i] = (ActionType) x.Next(5);
                }

                if (VerifyRoundPlay(enemy)) playIsValid = true;
            }

            return enemy;
        }
        
        // all methods below are either copy/paste or modification
        // of the functions of 
        private bool VerifyRoundPlay(Player player)
        {
                int energy = 7;

                for (int i = 0; i <= 4; i++)
                {
                    // Verify player action choices pt 1
                    ActionType actionType = player.actions[i];
                    if (i > 0)
                    {
                       ActionType prevActionType = player.actions[i - 1];
                       if (actionType == ActionType.HeavySwordS &&
                           prevActionType != ActionType.HeavySwordH)
                       {
                           return false;
                       }
                    }

                    // Verify player action choices pt 2
                    if (i < 4)
                    {
                        ActionType nextActionType = player.actions[i + 1];
                        if (actionType == ActionType.HeavySwordH &&
                            nextActionType != ActionType.HeavySwordS)
                        {
                            return false;
                        }
                    }
                    
                    // calculate energy usage
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

                // verify energy usage
                if (energy < 0) return false;
                return true;
            
        }

        private int CalcDamage(ActionType attackAction, ActionType defendAction)
        {
            switch (attackAction)
            {
                case ActionType.Shield:
                    return 0;
                case ActionType.Sword:
                    if (defendAction == ActionType.Shield)
                        return 0;
                    else if (defendAction == ActionType.HeavySwordH)
                        return 2;
                    else
                        return 1;
                case ActionType.HeavySwordS:
                    if (defendAction == ActionType.Shield)
                        return 1;
                    else if (defendAction == ActionType.HeavySwordH)
                        return 3;
                    else
                        return 2;
                case ActionType.HeavySwordH:
                    return 0;
            }

            return 0;
        }
        private GameRound DetermineRound(Player p1, Player p2)
        {
            int p1D = 0;
            int p2D = 0;
            for (int i = 0; i <= 4; i++)
            {
                p1D += CalcDamage((ActionType)p1.actions[i], (ActionType)p2.actions[i]);
                p2D += CalcDamage((ActionType)p2.actions[i], (ActionType)p1.actions[i]);
            }

            int w = -1;
            if (p1D > p2D) w = 1;
            else if (p1D < p2D) w = 2;
            else w = 3;
            p1.lockedIn = false;
            p2.lockedIn = false;
            GameRound gameRound = new GameRound {player1 = p1, player2 = p2, player1Damnage = p1D, player2Damage = p2D, winner = w};
            return gameRound;
        }
    }
}