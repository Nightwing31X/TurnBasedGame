using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GameDev;
using UnityEngine.UI;
using System.Runtime.CompilerServices;


namespace Menu
{
    [AddComponentMenu("GameDev/Menu/MainMenu Events")]
    public class MainMenuEvents : MonoBehaviour
    {
        public GameObject currentButton, menuFirstButton, settingsFirstButton, settingsButton;

        // public GameObject playercharacterMenu;

        public Toggle maleToggle;
        public Toggle femaleToggle;

        public SavePlayerData savePlayerData;

        private GameObject _maleCharacter;
        private GameObject _femaleCharacter;
        private GameObject _maleDisplayUsernameText;
        private GameObject _femaleDisplayUsernameText;

        public void ChangeScene(int sceneNumber)
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 0)
            {
                StartCoroutine(Delay(sceneNumber));
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

            _maleCharacter = GameObject.Find("MaleCharacter");
            _femaleCharacter = GameObject.Find("FemaleCharacter");

            _maleDisplayUsernameText = GameObject.Find("maleDisplayUsernameTEXT");
            _femaleDisplayUsernameText = GameObject.Find("femaleDisplayUsernameTEXT");

            SelectObjectUI();
        }

        void Start()
        {
            if (savePlayerData.maleREF)
            {
                maleToggle.isOn = true;
                femaleToggle.isOn = false;
                _maleCharacter.SetActive(true);
                _femaleCharacter.SetActive(false);
                SetActiveRecursively(_femaleCharacter.transform, false);
                _maleDisplayUsernameText.GetComponent<Text>().text = savePlayerData.usernameREF;
            }
            else
            {
                femaleToggle.isOn = true;
                maleToggle.isOn = false;
                _femaleCharacter.SetActive(true);
                _maleCharacter.SetActive(false);
                SetActiveRecursively(_maleCharacter.transform, false);
                _femaleDisplayUsernameText.GetComponent<Text>().text = savePlayerData.usernameREF;
            }



        }

        void SetActiveRecursively(Transform target, bool isActive)
        {
            // Set the current GameObject's active state
            target.gameObject.SetActive(isActive);

            // Loop through all children of the target
            for (int i = 0; i < target.childCount; i++)
            {
                SetActiveRecursively(target.GetChild(i), isActive);
            }
        }

        public void SelectObjectUI()
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(menuFirstButton);
        }

        // public void OpenFirstPlayMenu()
        // {
        //     if (savePlayerData.firstTimeREF)
        //     {
        //         Debug.Log("Have saved file - Username is already done, just load in to the game.");
        //         ChangeScene(1);
        //     }
        //     else
        //     {
        //         Debug.Log("First time playing; allow player to create character.");
        //         playercharacterMenu.SetActive(true);
        //     }
        // }

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
                _femaleDisplayUsernameText.GetComponent<Text>().text = name;

                femaleToggle.isOn = true;
                _femaleCharacter.SetActive(true);
                SetActiveRecursively(_femaleCharacter.transform, true);
                _maleCharacter.SetActive(false);
                SetActiveRecursively(_maleCharacter.transform, false);

                savePlayerData.GenderToggle(false);
            }
            else if (maleToggle.isOn == true)
            {
                _maleDisplayUsernameText.GetComponent<Text>().text = name;

                femaleToggle.isOn = false;
                _maleCharacter.SetActive(true);
                SetActiveRecursively(_maleCharacter.transform, true);
                _femaleCharacter.SetActive(false);
                SetActiveRecursively(_femaleCharacter.transform, false);
                savePlayerData.GenderToggle(true);
            }
        }
        public void ToggleFemale()
        {
            if (femaleToggle.isOn == false)
            {
                _maleDisplayUsernameText.GetComponent<Text>().text = name;
                
                maleToggle.isOn = true;
                _maleCharacter.SetActive(true);
                SetActiveRecursively(_maleCharacter.transform, true);
                _femaleCharacter.SetActive(false);
                SetActiveRecursively(_femaleCharacter.transform, false);
                savePlayerData.GenderToggle(true);

            }
            else if (femaleToggle.isOn == true)
            {
                _femaleDisplayUsernameText.GetComponent<Text>().text = name;

                maleToggle.isOn = false;
                _femaleCharacter.SetActive(true);
                SetActiveRecursively(_femaleCharacter.transform, true);
                _maleCharacter.SetActive(false);
                SetActiveRecursively(_maleCharacter.transform, false);
                savePlayerData.GenderToggle(false);
            }
        }

        public void SetUsername(string name)
        {
            if (savePlayerData.maleREF)
            {
                _maleDisplayUsernameText.GetComponent<Text>().text = name;
            }
            else
            {
                _femaleDisplayUsernameText.GetComponent<Text>().text = name;
            }
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