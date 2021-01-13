using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Data;
using CardGame.Management;
using CardGameShared.Data;
using UnityEngine;
using UnityEngine.UI;
using Avatar = CardGameShared.Data.Avatar;

public class BattleSceneManager : MonoBehaviour
{
    private PlayManager _playManager;
    private Player _enemy;
    private Player _me;
    private int _turn = -1;
    [SerializeField] private Image enemyImage;
    [SerializeField] private Image meImage;
    [SerializeField] private Image enemyAction;
    [SerializeField] private Image meAction;
    [SerializeField] private Sprite[] actionImagees = new Sprite[4];
    [SerializeField] private Sprite[] avatars = new Sprite[6];
    [SerializeField] private Image[] enemyDamageImages = new Image[5];
    [SerializeField] private int enemyDamage = 0;
    [SerializeField] private int meDamage = 0;
    GameTurn[] turns = new GameTurn[5];
    private void Awake()
    {
        try
        {
            _playManager = GameObject.Find("PlayManager").GetComponent<PlayManager>();
            Debug.Log("Is real game...waiting on play manager to send data.");
        }
        catch
        {
            Debug.Log("Is not real game...loading simulation");
            Player me = new Player
            {
                actions = new[]
                {
                    ActionType.Shield, ActionType.Shield, ActionType.Sword, ActionType.HeavySwordH,
                    ActionType.HeavySwordS
                },
                name = "Me",
                avatar = Avatar.TVGuy,
                lockedIn = true,
                sessionId = "testuser1"
            };
            GameRound gameRound = new GameRound
            {
                player1 = me,
                player1Damnage = 4,
                player2 = new Player
                {
                    actions = new [] {ActionType.HeavySwordH, ActionType.HeavySwordS, ActionType.Shield, ActionType.Shield, ActionType.Shield},
                    name = "Enemy",
                    avatar = Avatar.BlueGuy,
                    lockedIn = true,
                    sessionId = "testuser2"
                },
                player2Damage = 2
            };
            RunBattleScreen(me, gameRound);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    internal void RunBattleScreen(Player rcvdMePlayer, GameRound gameRound)
    {
        if (gameRound.player1.sessionId == rcvdMePlayer.sessionId)
        {
            _enemy = gameRound.player2;
            _me = gameRound.player1;
        }
        else if (gameRound.player2.sessionId == rcvdMePlayer.sessionId)
        {
            _enemy = gameRound.player2;
            _me = gameRound.player1;
        }
        for (int i = 0; i < 5; i++)
        {
            turns[i] = new GameTurn
            {
                damageDealt = CalcDamage(_me.actions[i], _enemy.actions[i]),
                damageReceived = CalcDamage(_enemy.actions[i], _me.actions[i]),
                action = _me.actions[i],
                actionRecieved = _enemy.actions[i]
            };
        }
        enemyImage.sprite = avatars[(int) _enemy.avatar];
        meImage.sprite = avatars[(int) _me.avatar];
        _turn = 0;
        DisplayTurn();
    }

    public void DisplayTurn()
    {
        if (_turn < 5)
        {
            Debug.Log("turn " + _turn);
            Debug.Log("enemy action " + _enemy.actions[_turn]);
            Debug.Log("my action" + _me.actions[_turn]);
            enemyAction.sprite = actionImagees[(int) _enemy.actions[_turn]];
            meAction.sprite = actionImagees[(int) _me.actions[_turn]];
            
            // TODO: Show damage
            
            _turn++;
        }
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
}
