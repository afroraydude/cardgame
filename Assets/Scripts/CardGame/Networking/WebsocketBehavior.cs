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
        private bool isOnline = true;
        private void Awake()
        {
            _playManager = GetComponent<PlayManager>();
            if (Application.isEditor)
            { 
                _domain = "192.168.100.9"; // localhost
            }
            else
            {
                _domain = "scratchbattle.afroraydude.com";
            }

            //_domain = "192.168.100.9";
            
            _dir = _playManager._manager.websocketDir;
            Debug.Log(_dir);
            if (_dir != "singleplayer")
            {
                _ws = new WebSocket($"ws://{_domain}/{_dir}");
                _ws.OnMessage += OnMessage;
                _ws.OnOpen += OnOpen;
                _ws.Connect();
            }
            else
            {
                isOnline = false;
            }
        }

        private void OnEnable()
        {
            
        }

        private void Start()
        {
            
        }
        
        void OnApplicationQuit()
        {
            //_ws.Close();
        }

        public void SendSocketMessage(ProperMessage message)
        {
            string msg = JsonConvert.SerializeObject(message);
            if (isOnline) _ws.Send(msg);
            if (!isOnline)
            {
                if (message.messageType == MessageType.RoundPlay)
                {
                    Player self = JsonConvert.DeserializeObject<Player>(message.messageData);
                    PlaySinglePlayerRound(self);
                }
            }
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
                    Debug.Log($"Game Round result received: {message.messageData}");
                    _playManager.ProcessRoundPlayed(JsonConvert.DeserializeObject<GameRound>(message.messageData));
                }

                if (message.messageType == MessageType.JoinAccept)
                {
                    _playManager.me = JsonConvert.DeserializeObject<Player>(message.messageData);
                    Debug.Log($"Got new version of me: {message.messageData}");
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

        void PlaySinglePlayerRound(Player self)
        {
            var play = _playManager.GenerateSinglePlayerPlay(self);
            _playManager.ProcessRoundPlayed(play);
        }
    }
}