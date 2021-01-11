using CardGame.Data;
using System.Collections;
using System.Collections.Generic;
using CardGame.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickAvatar : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Game(int choice)
    {
        SaveFile saveFile = gameManager.LoadSaveFile();
        switch (choice)
        {
            case (int)CardGameShared.Data.Avatar.BaldGuy:
                saveFile.avatar = CardGameShared.Data.Avatar.BaldGuy;
                break;
            case (int)CardGameShared.Data.Avatar.PinkHairGirl:
                saveFile.avatar = CardGameShared.Data.Avatar.PinkHairGirl;
                break;
            case (int)CardGameShared.Data.Avatar.BlueGuy:
                saveFile.avatar = CardGameShared.Data.Avatar.BlueGuy;
                break;
            case (int)CardGameShared.Data.Avatar.HelmetGuy:
                saveFile.avatar = CardGameShared.Data.Avatar.HelmetGuy;
                break;
            case (int)CardGameShared.Data.Avatar.TVGuy:
                saveFile.avatar = CardGameShared.Data.Avatar.TVGuy;
                break;
            case (int)CardGameShared.Data.Avatar.BrownHOrangeSGirl:
                saveFile.avatar = CardGameShared.Data.Avatar.BrownHOrangeSGirl;
                break;
        }

        gameManager.WriteSaveToFile(saveFile);
        
        //write functionality to also change scene to "Game", OnClick
        SceneManager.LoadScene("Game");

    }
}
