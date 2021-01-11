using System.Collections;
using System.Collections.Generic;
using CardGame.Data;
using CardGame.Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IDJoinManager : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;

    private string id = "000000";
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void NameEntry(string name)
    {
        id = name;
    }

    public void OnContinue()
    {
        gameManager.websocketDir = id;
        SceneManager.LoadScene("Enter Username");
    }
}
