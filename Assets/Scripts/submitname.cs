using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class submitname : MonoBehaviour
{
    public void submitName()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
}
