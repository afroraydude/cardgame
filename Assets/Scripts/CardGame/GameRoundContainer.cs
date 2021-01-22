using System.Collections;
using System.Collections.Generic;
using CardGameShared.Data;
using UnityEngine;

public class GameRoundContainer : MonoBehaviour
{
    public GameRound gameRound;

    public Player me;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
