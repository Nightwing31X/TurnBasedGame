using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GameDev;
using UnityEngine.UI;


namespace Menu
{
    [AddComponentMenu("GameDev/Menu/MainMenu Events")]
    public class MainMenuEvents : MonoBehaviour
    {
        public GameObject currentButton, menuFirstButton, settingsFirstButton, settingsButton;

        public GameObject playercharacterMenu;

        public Toggle maleToggle;
        public Toggle femaleToggle;

        public SavePlayerData savePlayerData;

        public void ChangeScene(int sceneNumber)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 0)
            {
                if (!savePlayerData.noName)
                {
                    StartCoroutine(Delay(sceneNumber));
                }
            }
            //SceneManager.LoadScene(sceneNumber);
        }
        public void ExitToDesktop()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        IEnumerator Delay(int num)
        {
            Debug.Log("Make the loading screen show here");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(num);
        }

        private void Awake()
        {
            savePlayerData = GetComponent<SavePlayerData>();

            playercharacterMenu.SetActive(false);

            SelectObjectUI();

            //if (maleToggle.isOn)
            //{
            //    ToggleMale();
            //}
            //else
            //{
            //    if (femaleToggle.isOn)
            //    {
            //        ToggleFemale();
            //    }
            //}
        }

        public void SelectObjectUI()
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(menuFirstButton);
        }

        public void OpenFirstPlayMenu()
        {
            if (savePlayerData.firstTimeREF)
            {
                Debug.Log("Have saved file - Username is already done, just load in to the game.");
                ChangeScene(1);
            }
            else
            {
                Debug.Log("First time playing; allow player to create character.");
                playercharacterMenu.SetActive(true);
            }
        }

        public void opensettingsMenu()
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(settingsFirstButton);
        }

        public void ToggleMale()
        {
            if (maleToggle.isOn == false)
            {
                femaleToggle.isOn = true;
                savePlayerData.GenderToggle(false);
                Debug.Log("This one...");
            }
            else if (maleToggle.isOn == true)
            {
                savePlayerData.GenderToggle(true);
                femaleToggle.isOn = false;
            }
        }
        public void ToggleFemale()
        {
            if (femaleToggle.isOn == false)
            {
                maleToggle.isOn = true;
                savePlayerData.GenderToggle(true);
            }
            else if (femaleToggle.isOn == true)
            {
                savePlayerData.GenderToggle(false);
                maleToggle.isOn = false;
            }
        }

        public void SetUsername(string name)
        {
            savePlayerData.SaveUsername(name);
            Debug.Log(name);
        }


        public void backsettingsMenu()
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(settingsButton);
        }
    }
}