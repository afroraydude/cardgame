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
    [SerializeField] private Text idText;
    private string _domain;
    void Start()
    {
        if (Application.isEditor)
        {
            _domain = "scratchbattle.afroraydude.com"; // localhost
        }
        else
        {
            _domain = "scratchbattle.afroraydude.com";
        }
        _ws = new WebSocket($"ws://{_domain}/lobby");
        _ws.OnMessage += (sender, e) =>
        {
            Debug.Log($"Received Message:\n{e.Data}");
            ProperMessage m = JsonConvert.DeserializeObject<ProperMessage>(e.Data);
            if (m.messageType == MessageType.CreateAccept)
            {
                Debug.Log(m.messageData);
                id = m.messageData;
                PlayerPrefs.SetString("game_id", id);
                PlayerPrefs.Save();
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
