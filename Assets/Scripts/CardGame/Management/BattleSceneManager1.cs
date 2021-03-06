using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Data;
using CardGame.Management;
using CardGameShared.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Avatar = CardGameShared.Data.Avatar;

public class BattleSceneManager1 : MonoBehaviour
{
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
    [SerializeField] private Image[] meDamageImages = new Image[5];
    /* Damage dealt by the enemy */
    [SerializeField] private int enemyDamage = 0;
    /* Damage dealt by me */
    [SerializeField] private int meDamage = 0;
    GameTurn[] turns = new GameTurn[5];

    [SerializeField] private Text nextButtonText;
    
    [SerializeField] private GameRoundContainer _gameRoundContainer;
    
    
    private void Start()
    {
        /*
        try
        {
            _gameRoundContainer = GameObject.Find("GameRound").GetComponent<GameRoundContainer>();
            RunBattleScreen(_gameRoundContainer.me, _gameRoundContainer.gameRound);
        }
        catch {
            Debug.Log("Is real game...waiting on play manager to send data.");
        
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
        */
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
            _enemy = gameRound.player1;
            _me = gameRound.player2;
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
            if (_enemy.actions == null)
            {
                Debug.Log("err1");
            }
        
            if (_me.actions == null)
            {
                Debug.Log("er2");
            }
            enemyAction.sprite = actionImagees[(int) _enemy.actions[_turn]];
            meAction.sprite = actionImagees[(int) _me.actions[_turn]];
            enemyDamage += CalcDamage(_enemy.actions[_turn], _me.actions[_turn]);
            meDamage += CalcDamage(_me.actions[_turn], _enemy.actions[_turn]);
            ShowDamage();
        }
        else
        {
            SceneManager.LoadScene("TitleScreen");
        }

        if (_turn == 4)
        {
            nextButtonText.text = "End Game";
            Debug.Log($"Enemy damage: {enemyDamage}");
            Debug.Log($"My damage: {meDamage}");
        }
        _turn++;
    }

    private void ShowDamage()
    {
        for (int i = 1; i <= enemyDamage; i++)
        {
            if (enemyDamage > 0)
                meDamageImages[i - 1].color = Color.red;
        }
        
        for (int i = 1; i <= meDamage; i++)
        {
            if (meDamage > 0)
                enemyDamageImages[i - 1].color = Color.red;
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
