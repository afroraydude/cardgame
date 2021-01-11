using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CardGame.Data;
using CardGameShared.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Avatar = CardGameShared.Data.Avatar;
using Random = System.Random;

namespace CardGame.Management
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] public string websocketDir;

        public SaveFile playerinfo;
        private void Awake()
        {
            /*
            if (GameObject.Find("GameManager") != null && GameObject.Find("GameManager") != gameObject)
            {
                gameObject.SetActive(false);
            }
            */
            SceneManager.sceneLoaded += OnSceneLoaded;

            List<RuntimePlatform> rp = new List<RuntimePlatform>
                {RuntimePlatform.LinuxPlayer, RuntimePlatform.WindowsPlayer, RuntimePlatform.OSXPlayer};
            if (rp.Contains(Application.platform))
            {
                Screen.SetResolution(405,720, FullScreenMode.Windowed);
            }

            SaveFile saveFile;
            if (!File.Exists("playerinfo.dat"))
            {
                saveFile = CreateNewSaveFile();
            }
            else
            {
                saveFile = LoadSaveFile();
            }

            playerinfo = saveFile;
            DontDestroyOnLoad(gameObject);
        }

        static string RandomString (int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789qwertyuiopasdfghjklzxcvbnm";
            
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        private SaveFile CreateNewSaveFile()
        {
            SaveFile newFile = new SaveFile {avatar = Avatar.BaldGuy, name = RandomString(16)};

            string saveFileString = JsonConvert.SerializeObject(newFile);
            File.WriteAllText("playerinfo.dat", saveFileString);

            return newFile;
        }

        public bool WriteSaveToFile(SaveFile file)
        {
            string saveFileString = JsonConvert.SerializeObject(file);
            File.WriteAllText("playerinfo.dat", saveFileString);

            return true;
        }

        public SaveFile LoadSaveFile()
        {
            string data = File.ReadAllText("playerinfo.dat");
            SaveFile output = JsonConvert.DeserializeObject<SaveFile>(data);
            return output;
        }
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
        }
    }
}