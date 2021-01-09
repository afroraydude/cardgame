using CardGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickAvatar : MonoBehaviour
{
    public void Game(int choice)
    {
        SaveFile saveFile = new SaveFile();
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
        }
        //write functionality to also change scene to "Game", OnClick
        SceneManager.LoadScene("Game");

    }
}
