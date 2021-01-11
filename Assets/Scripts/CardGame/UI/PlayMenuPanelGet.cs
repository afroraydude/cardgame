using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Management;
using CardGame.Networking;
using CardGameShared.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class PlayMenuPanelGet : MonoBehaviour
{
    // Start is called before the first frame update
    private string id = "Loading ID";
    [SerializeField] private Button continueButton;
    private WebSocket _ws;
    private GameManager _gameManager;
    [SerializeField] private Text idText;
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _ws = new WebSocket($"ws://localhost:5001/lobby");
        _ws.OnMessage += (sender, e) =>
        {
            Debug.Log($"Received Message:\n{e.Data}");
            ProperMessage m = JsonConvert.DeserializeObject<ProperMessage>(e.Data);
            if (m.messageType == MessageType.CreateAccept)
            {
                Debug.Log(m.messageData);
                id = m.messageData;
                _gameManager.websocketDir = id;
            }
        };
        _ws.OnOpen += OnOpen;
        _ws.OnClose += OnClose;
        _ws.Log.Level = LogLevel.Debug;
        _ws.Connect();
        /*
        using (var ws = new WebSocket("ws://localhost:5001/lobby"))
        {
            ws.OnMessage += (sender, e) =>
            {
                Console.WriteLine($"Received Message:\n{e.Data}");
                ProperMessage m = JsonConvert.DeserializeObject<ProperMessage>(e.Data);
                if (m.messageType == MessageType.CreateAccept)
                {
                    idText.text = m.messageData;
                }
            };
            ws.Connect();
            ProperMessage m = new ProperMessage {messageData = String.Empty, messageType = MessageType.Create};
            ws.Send(JsonConvert.SerializeObject(m));
        }
        */
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        
    }

    private void OnOpen(object sender, EventArgs e)
    {
        ProperMessage m = new ProperMessage {messageData = String.Empty, messageType = MessageType.Create};
        _ws.Send(JsonConvert.SerializeObject(m));
    }

    private void OnClose(object sender, EventArgs e)
    {
        //_ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        idText.text = id;
    }

    private void OnDestroy()
    {
        _ws.Close();
    }
}
