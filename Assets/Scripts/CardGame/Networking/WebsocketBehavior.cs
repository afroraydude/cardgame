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
        public WebSocket _ws;
        private string _dir;
        private PlayManager _playManager;
        private string _domain;
        private void Awake()
        {
            _playManager = GetComponent<PlayManager>();
            if (Application.isEditor)
            {
                _domain = "scratchbattle.afroraydude.com"; // localhost
            }
            else
            {
                _domain = "scratchbattle.afroraydude.com";
            }
        }

        private void OnEnable()
        {
            
        }

        private void Start()
        {
            _dir = PlayerPrefs.GetString("game_id", "DebugGame");
            _ws = new WebSocket($"ws://{_domain}/{_dir}");
            _ws.OnMessage += OnMessage;
            _ws.OnOpen += OnOpen;
            _ws.Connect();
        }
        
        void OnApplicationQuit()
        {
            //_ws.Close();
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
            if (_dir != "lobby")
            {
                ProperMessage message = LoadMessage(e.Data);
                if (message.messageType == MessageType.RoundResult)
                {
                    _playManager.ProcessRoundPlayed(JsonConvert.DeserializeObject<GameRound>(message.messageData));
                }

                if (message.messageType == MessageType.JoinAccept)
                {
                    _playManager.me = JsonConvert.DeserializeObject<Player>(message.messageData);
                }

                if (message.messageType == MessageType.RoundPlayAccept)
                {
                    _playManager.WaitOnOpponent();
                }
                if (message.messageType == MessageType.RoundPlayDeny)
                {
                    _playManager.ResetActions();
                }
            }
        }
        private void OnOpen(object sender, EventArgs e)
        {
                Player me = _playManager.me;

                ProperMessage message = new ProperMessage
                {
                    messageType = MessageType.Join,
                    messageData = JsonConvert.SerializeObject(me)
                };
                SendSocketMessage(message);
        }
    }
}