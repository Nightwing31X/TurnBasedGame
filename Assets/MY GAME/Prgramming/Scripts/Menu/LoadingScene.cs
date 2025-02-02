using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using GameDev;

namespace Menu
{
    [AddComponentMenu("GameDev/Menu/Loading Scene")]
    public class LoadingScene : MonoBehaviour
    {
        public GameObject loadingScenePanel;
        public Image progressBar;
        public Text progressText;

        public SavePlayerData savePlayerData;
        public GameObject playercharacterMenu;


        IEnumerator LoadAsynchronously(int sceneIndex)
        {
            loadingScenePanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                progressBar.fillAmount = progress;
                // progressText.text = $"{progress*100:P0}";
                progressText.text = $"{progress:P0}";
                yield return null;
            }
        }

        public void LoadLevelAsync(int sceneIndex)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }


        void Awake()
        {
            savePlayerData = GetComponent<SavePlayerData>();
            playercharacterMenu.SetActive(false);
        }

        public void OpenFirstPlayMenu()
        {
            if (savePlayerData.firstTimeREF == false)
            {
                Debug.Log("Have saved file - Username is already done, just load in to the game.");
                LoadLevelAsync(1);
            }
            else
            {
                playercharacterMenu.SetActive(true);
                playercharacterMenu.transform.Find("AllButtons").GetComponent<Animator>().SetBool("playercharacterMenuOpen", true);
                savePlayerData.firstTimeREF = false;
                Debug.Log("First time playing; allow player to create character.");
            }
        }
    }
}


// {"firstTime":false,"DropdownValue":3,"playerName":"Nightwing31X","male":true,"level":0,"swordPurple":false,"shieldWood":true,"currentHealth":100,"maxHealth":100,"currentBagValue":0,"maxBagValue":24}