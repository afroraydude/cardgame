using CardGame.Data;
using System.Collections;
using System.Collections.Generic;
using CardGame.Management;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickAvatar : MonoBehaviour
{
    public void Start()
    {
    }
    public void Game(int choice)
    {
        SaveFile saveFile = JsonConvert.DeserializeObject<SaveFile>(PlayerPrefs.GetString("save"));
        Debug.Log(JsonConvert.SerializeObject(saveFile));
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

        string data = JsonConvert.SerializeObject(saveFile);
        PlayerPrefs.SetString("save", data);
        PlayerPrefs.Save();
        Debug.Log(JsonConvert.SerializeObject(saveFile));
        //write functionality to also change scene to "Game", OnClick
        SceneManager.LoadScene("Game");

    }
}
