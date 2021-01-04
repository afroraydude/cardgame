using System;
using CardGame.Management;
using UnityEngine;
using CardGameShared.Data;
using Newtonsoft.Json;
using WebSocketSharp;

namespace CardGame.Networking
{
    public class WebsocketBehavior : MonoBehaviour
    {
        private WebSocket _ws;
        private string _dir;
        private PlayManager _playManager;

        private void Awake()
        {
            _playManager = GetComponent<PlayManager>();
        }

        private void Start()
        {
            _dir = _playManager.gameManager.websocketDir;
            _ws = new WebSocket($"ws://localhost:5001/{_dir}");
            _ws.OnMessage += OnMessage;
            _ws.OnOpen += OnOpen;
            _ws.Connect();
        }
        
        void OnApplicationQuit()
        {
            _ws.Close();
        }

        public void SendSocketMessage(ProperMessage message)
        {
            string msg = JsonConvert.SerializeObject(message);
            _ws.Send(msg);
        }

        private ProperMessage LoadMessage(string rawMessage)
        {
            Debug.Log($"Received Message:\n{rawMessage}");
            ProperMessage recvmsg = JsonConvert.DeserializeObject<ProperMessage>(rawMessage);
            return recvmsg;
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            ProperMessage message = LoadMessage(e.Data);
            if (message.messageType == MessageType.RoundResult)
            {
                _playManager.ProcessRoundPlayed(JsonConvert.DeserializeObject<GameRound>(message.messageData));
            }
        }
        private void OnOpen(object sender, EventArgs e)
        {
            if (_dir == "lobby")
            {
                
            }
            else
            {
                // is game
                Player me = new Player
                {
                    actions = new[]
                    {
                        ActionType.NullAction, ActionType.NullAction, ActionType.NullAction,
                        ActionType.NullAction, ActionType.NullAction
                    },
                    lockedIn = false,
                    avatar = _playManager.gameManager.playerinfo.avatar,
                    name = _playManager.gameManager.playerinfo.name
                };

                ProperMessage message = new ProperMessage
                {
                    messageType = MessageType.Join,
                    messageData = JsonConvert.SerializeObject(me)
                };
                SendSocketMessage(message);
            }
        }
    }
}