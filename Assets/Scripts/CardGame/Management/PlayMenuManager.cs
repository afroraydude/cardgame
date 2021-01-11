using System;
using System.Collections;
using System.Collections.Generic;
using CardGame.Management;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour
{
    public GameObject Panel;
    [SerializeField] public GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPanel()
    {
        if (Panel != null)
        {
            gameManager.websocketDir = "lobby";
            Panel.SetActive(true);
        }
    }
    
    public void JoinButton()
    {
        SceneManager.LoadScene("EnterIDJoin");
    }

    public void HostButton()
    {
        SceneManager.LoadScene("Enter Username");
    }
}
