using System;
using System.IO;
using System.Linq;
using CardGame.Data;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;
using Avatar = CardGameShared.Data.Avatar;
using Random = System.Random;

namespace CardGame.Management
{
    public class SettingsManager : MonoBehaviour
    {
        SaveFile _saveFile;
        [SerializeField] private InputField currentName; 
        
        void Start()
        {
            if (!File.Exists("playerinfo.dat"))
            {
                _saveFile = CreateNewSaveFile();
            }
            else
            {
                _saveFile = LoadSaveFile();
            }
            Debug.Log(_saveFile.name);
            if (currentName.text == "") currentName.text += _saveFile.name;
        }
        
        public SaveFile LoadSaveFile()
        {
            string data = File.ReadAllText("playerinfo.dat");
            SaveFile output = JsonConvert.DeserializeObject<SaveFile>(data);
            return output;
        }
        
        static string RandomString (int length)
        {
            System.Random random = new Random();
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

        public void UpdateName(string newName)
        {
            _saveFile.name = newName;
        }

        public void UpdateAvatar(int choice)
        {
            Avatar newAvatar = (Avatar) choice;
            _saveFile.avatar = newAvatar;
        }
        
        public void WriteSaveToFile()
        {
            if (_saveFile.name.IsNullOrEmpty()) _saveFile.name = RandomString(16);
            string saveFileString = JsonConvert.SerializeObject(_saveFile);
            File.WriteAllText("playerinfo.dat", saveFileString);

            SceneManager.LoadScene("Lobby");
        }
    }
}