using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playbutton : MonoBehaviour
{
    public void playButton()
    {
        SceneManager.LoadScene("PlayMenu");
    }

    public void Rules()
    {
        SceneManager.LoadScene("Rules");
    }
}
