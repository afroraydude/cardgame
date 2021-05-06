using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip[] bgmSounds;
    [SerializeField] private AudioClip choice;
    [SerializeField] private AudioClip lastChoice;
    private AudioSource _source;
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        DontDestroyOnLoad(gameObject);
        _source = GetComponent<AudioSource>();
        _source.loop = true;
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            OnSceneLoad(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        lastChoice = choice;
        /*
        if (scene.name == "TitleScreen")
        {
            choice = bgmSounds[0];
        }

        if (scene.name == "Game")
        {
            choice = bgmSounds[1];
        }

        if (scene.name == "Rules")
        {
            choice = bgmSounds[2];
        }

        if (scene.name == "BattleScene")
        {
            choice = bgmSounds[3];
        }
        */

        switch (scene.name)
        {
            case "TitleScreen":
                choice = bgmSounds[0];
                break;
            case "Game":
                choice = bgmSounds[1];
                break;
            case "Rules":
                choice = bgmSounds[2];
                break;
            case "BattleScene":
                choice = bgmSounds[3];
                break;
        }
        choice.LoadAudioData();
        _source.clip = choice;
        if (lastChoice != choice)
            _source.Play();
    }
}
