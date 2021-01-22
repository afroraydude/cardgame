using System.Collections;
using System.Collections.Generic;
using CardGame.Data;
using CardGame.Management;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UsernameEntryManager : MonoBehaviour
{

    [SerializeField] private InputField nameChoice;
    // Start is called before the first frame update
    private SaveFile gameSave;
    void Awake()
    {
        gameSave = JsonConvert.DeserializeObject<SaveFile>(PlayerPrefs.GetString("save"));
        nameChoice.text = gameSave.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void NameEntry(string name)
    {
        gameSave.name = name;
    }

    public void OnContinue()
    {
        string data = JsonConvert.SerializeObject(gameSave);
        PlayerPrefs.SetString("save", data);
        PlayerPrefs.Save();
        SceneManager.LoadScene("CharacterSelect");
    }
}
