using System.Collections;
using System.Collections.Generic;
using CardGame.Data;
using CardGame.Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UsernameEntryManager : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;

    [SerializeField] private InputField nameChoice;
    // Start is called before the first frame update
    private SaveFile gameSave;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameSave = gameManager.LoadSaveFile();
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
        gameManager.WriteSaveToFile(gameSave);
        SceneManager.LoadScene("CharacterSelect");
    }
}
