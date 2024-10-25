using System;
using System.IO;
using UnityEngine;

namespace GameDev
{
    [AddComponentMenu("GameDev/Save Player Data")]
    public class SavePlayerData : MonoBehaviour
    {
        [SerializeField] private PlayerDataSave playerDataSave = new PlayerDataSave();

        public bool firstTImeREF;

        public string usernameREF;
        public bool maleREF;
        public int levelREF;
        public bool swordPurpleREF;
        public bool shieldWoodREF;

        [ContextMenu("Save")]
        public void Save()
        {
            RefValues();
            string json = JsonUtility.ToJson(playerDataSave);
            File.WriteAllText($"{Application.persistentDataPath}/PlayerDataSaved.txt", json);
            Debug.Log("Player data saved.");
        }

        [ContextMenu("Load")]
        public void Load()
        {
            string path = $"{Application.persistentDataPath}/PlayerDataSaved.txt";

            if (!File.Exists(path))
            {
                playerDataSave = new PlayerDataSave(); // Initialise with default values
                playerDataSave.firstTime = true;
                Save(); // Create the new file
                Debug.Log("File not found. Created new save file with default values.");
                return;
            }

            string json = File.ReadAllText(path);
            playerDataSave = JsonUtility.FromJson<PlayerDataSave>(json);
            SavedValues();
            Debug.Log("Loaded player data.");
        }

        void Awake()
        {
            Load();
            Debug.Log("SavedPlayerData script has run...");
        }

        public void RefValues()
        {
            Debug.Log(playerDataSave.firstTime);
            playerDataSave.firstTime = firstTImeREF;
            Debug.Log(playerDataSave.firstTime);
            Debug.Log(firstTImeREF);

            playerDataSave.playerName = usernameREF;
            playerDataSave.male = maleREF;
            playerDataSave.level = levelREF;
            playerDataSave.swordPurple = swordPurpleREF;
            playerDataSave.shieldWood = shieldWoodREF;

            //Debug.Log($"{usernameREF}: {maleREF}: {levelREF}: {swordPurpleREF}: {shieldWoodREF}");
        }

        public void SavedValues()
        {
            firstTImeREF = playerDataSave.firstTime;

            usernameREF = playerDataSave.playerName;
            maleREF = playerDataSave.male;
            levelREF = playerDataSave.level;
            swordPurpleREF = playerDataSave.swordPurple;
            shieldWoodREF = playerDataSave.shieldWood;

            Debug.Log("SavedValues has run...");
            //Debug.Log($"{playerDataSave.playerName}: {playerDataSave.male}: {playerDataSave.level}: {playerDataSave.swordPurple}: {playerDataSave.shieldWood}");
        }

        public void GenderToggle(bool check)
        {
            maleREF = check;
        }

        public void SaveUsername(string username)
        {
            usernameREF = username;
        }
    }

    [Serializable]
    public class PlayerDataSave
    {
        [SerializeField] public bool firstTime = false;
        [SerializeField] public string playerName = "The Bob";
        [SerializeField] public bool male = true;
        [SerializeField] public int level = 0;
        [SerializeField] public bool swordPurple = false;
        [SerializeField] public bool shieldWood = true;
    }
}
