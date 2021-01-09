using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titletomain : MonoBehaviour
{
    public void titleToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

