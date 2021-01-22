using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class titletomain : MonoBehaviour
{
    [SerializeField] private Text versionText;

    private void Start()
    {
        versionText.text = $"v{Application.version}";
    }

    public void titleToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

