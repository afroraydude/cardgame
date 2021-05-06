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


public class BattleSceneManager : MonoBehaviour
{

	#region variables definition
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
	#endregion 

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        _gameRoundContainer = GameObject.Find("GameRound").GetComponent<GameRoundContainer>();
        RunBattleScreen(_gameRoundContainer.me, _gameRoundContainer.gameRound);
    }

    // Update is called once per frame
    void Update()
    {
    }

    internal void RunBattleScreen(Player rcvdMePlayer, GameRound gameRound)
    {
        string test = JsonConvert.SerializeObject(gameRound);
        _enemy = gameRound.player2;
        _me = gameRound.player1;


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

            Debug.Log("turn " + _turn);
            if (_enemy.actions != null)
            {
                Debug.Log("eat " + (int) _enemy.actions[_turn]);

                enemyAction.sprite = actionImagees[(int) _enemy.actions[_turn]];
                if (_me.actions != null)
                {
                    meAction.sprite = actionImagees[(int) _me.actions[_turn]];
                    enemyDamage += CalcDamage(_enemy.actions[_turn], _me.actions[_turn]);
                    meDamage += CalcDamage(_me.actions[_turn], _enemy.actions[_turn]);
                }
                else
                {
                    Debug.Log("me actions is null");
                }
            }
            else
            {
                Debug.Log("enemy actions is null");
            }
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
