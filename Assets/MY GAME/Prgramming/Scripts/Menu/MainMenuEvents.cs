using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GameDev;
using UnityEngine.UI;
using System.Globalization;


namespace Menu
{
    [AddComponentMenu("GameDev/Menu/MainMenu Events")]
    public class MainMenuEvents : MonoBehaviour
    {
        public GameObject currentButton, menuFirstButton, settingsFirstButton, settingsButton;

        public GameObject playercharacterMenu;

        public Toggle GenderToggle;

        public SavePlayerData savePlayerData;

        public void ChangeScene(int sceneNumber)
        {
            StartCoroutine(Delay(sceneNumber));
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
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(num);
        }

        private void Awake()
        {
            savePlayerData = GetComponent<SavePlayerData>();

            playercharacterMenu.SetActive(false);

            SelectObjectUI();

            if (GenderToggle.isOn)
            {
                ToggleGender(GenderToggle.isOn);
            }
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
            if (savePlayerData.firstTImeREF)
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

        public void ToggleGender(bool check)
        {
            savePlayerData.GenderToggle(check);
            Debug.Log(check);
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