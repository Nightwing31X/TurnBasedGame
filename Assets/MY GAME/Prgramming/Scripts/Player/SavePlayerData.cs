using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameDev
{
    [AddComponentMenu("GameDev/Save Player Data")]
    public class SavePlayerData : MonoBehaviour
    {
        [SerializeField] private PlayerDataSave playerDataSave = new PlayerDataSave();

        public bool firstTimeREF = false;

        public string usernameREF = "The Bob";
        public bool maleREF;
        public int levelREF = 0;
        public bool swordPurpleREF;
        public bool shieldWoodREF;
        public int currentHealthREF;
        public int maxHealthREF;

        private bool hasRan;

        [ContextMenu("Save")]
        public void Save()
        {
            if (firstTimeREF)
            {
                FirstTimeRefValues();
            }
            else
            {
                RefValues();
            }
            if (usernameREF == "")
            {
                Debug.Log("Need to enter a name!");
                if (!hasRan)
                {
                    hasRan = true;
                    string json = JsonUtility.ToJson(playerDataSave);
                    File.WriteAllText($"{Application.persistentDataPath}/PlayerDataSaved.txt", json);
                    Debug.Log("Player data saved.");
                }
            }
            else
            {
                string json = JsonUtility.ToJson(playerDataSave);
                File.WriteAllText($"{Application.persistentDataPath}/PlayerDataSaved.txt", json);
                Debug.Log("Player data saved.");
            }
        }

        [ContextMenu("Load")]
        public void Load()
        {
            string path = $"{Application.persistentDataPath}/PlayerDataSaved.txt";

            if (!File.Exists(path))
            {
                playerDataSave = new PlayerDataSave(); // Initialise with default values
                firstTimeREF = true;
                Save(); // Create the new file
                Debug.Log("File not found. Created new save file with default values.");
                return;
            }

            string json = File.ReadAllText(path);
            playerDataSave = JsonUtility.FromJson<PlayerDataSave>(json);
            SavedValues();
            Debug.Log("Loaded player data.");
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 1)
            {
                Debug.Log(shieldWoodREF);
                Debug.Log("Runs only in the game...");
                PlayerCharacterManager.Instance.updatePlayerINFO();
            }
        }

        void Awake()
        {
            Load();
            Debug.Log("SavedPlayerData script has run...");
        }

        public void FirstTimeRefValues()
        {
            playerDataSave.firstTime = firstTimeREF;

            usernameREF = playerDataSave.playerName;
            // Debug.Log(playerDataSave.playerName);
            maleREF = playerDataSave.male;
            // Debug.Log(playerDataSave.male);
            levelREF = playerDataSave.level;
            // Debug.Log(playerDataSave.level);
            swordPurpleREF = playerDataSave.swordPurple;
            // Debug.Log(playerDataSave);
            shieldWoodREF = playerDataSave.shieldWood;
            // Debug.Log(playerDataSave.shieldWood);
            currentHealthREF = playerDataSave.currentHealth;
            maxHealthREF = playerDataSave.maxHealth;
        }

        public void RefValues()
        {
            playerDataSave.firstTime = firstTimeREF;

            playerDataSave.playerName = usernameREF;
            playerDataSave.male = maleREF;
            playerDataSave.level = levelREF;
            playerDataSave.swordPurple = swordPurpleREF;
            playerDataSave.shieldWood = shieldWoodREF;
            playerDataSave.maxHealth = maxHealthREF;
            playerDataSave.currentHealth = currentHealthREF;

            //Debug.Log($"{usernameREF}: {maleREF}: {levelREF}: {swordPurpleREF}: {shieldWoodREF}");
        }

        public void SavedValues()
        {
            firstTimeREF = playerDataSave.firstTime;

            usernameREF = playerDataSave.playerName;
            maleREF = playerDataSave.male;
            levelREF = playerDataSave.level;
            swordPurpleREF = playerDataSave.swordPurple;
            shieldWoodREF = playerDataSave.shieldWood;
            currentHealthREF = playerDataSave.currentHealth;
            maxHealthREF = playerDataSave.maxHealth;

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
        [SerializeField] public bool swordPurple = true;
        [SerializeField] public bool shieldWood = false;
        [SerializeField] public int currentHealth = 100;
        [SerializeField] public int maxHealth = 100;
    }
}
