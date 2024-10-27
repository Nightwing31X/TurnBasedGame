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

        [Header("Male Player Objects")]
        private GameObject playerWoodenShieldMALE;
        private GameObject playerYellowShieldMALE;
        private GameObject playerPurpleSwordMALE;
        private GameObject playerGreenSwordMALE;
        [Header("Female Player Objects")]
        private GameObject playerWoodenShieldFEMALE;
        private GameObject playerYellowShieldFEMALE;
        private GameObject playerPurpleSwordFEMALE;
        private GameObject playerGreenSwordFEMALE;
        [SerializeField] private Dropdown valueDropdown;

        [SerializeField] private bool shieldWood = false;
        [SerializeField] private bool swordPurple = true;

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

            // Male Objects - Shield
            playerWoodenShieldMALE = GameObject.Find("WoodenShieldMALE");
            playerYellowShieldMALE = GameObject.Find("YellowShieldMALE");
            // Male Objects - Sword
            playerPurpleSwordMALE = GameObject.Find("PurpleSwordMALE");
            playerGreenSwordMALE = GameObject.Find("GreenSwordMALE");

            // Female Objects - Shield
            playerWoodenShieldFEMALE = GameObject.Find("WoodenShieldFEMALE");
            playerYellowShieldFEMALE = GameObject.Find("YellowShieldFEMALE");
            // Female Objects - Sword
            playerPurpleSwordFEMALE = GameObject.Find("PurpleSwordFEMALE");
            playerGreenSwordFEMALE = GameObject.Find("GreenSwordFEMALE");

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

            // updatePlayerINFO();
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

                femaleToggle.isOn = true;
                _femaleCharacter.SetActive(true);
                SetActiveRecursively(_femaleCharacter.transform, true);
                _femaleDisplayUsernameText.GetComponent<Text>().text = savePlayerData.usernameREF;
                _maleCharacter.SetActive(false);
                SetActiveRecursively(_maleCharacter.transform, false);

                savePlayerData.GenderToggle(false);
            }
            else if (maleToggle.isOn == true)
            {

                femaleToggle.isOn = false;
                _maleCharacter.SetActive(true);
                SetActiveRecursively(_maleCharacter.transform, true);
                _maleDisplayUsernameText.GetComponent<Text>().text = savePlayerData.usernameREF;

                _femaleCharacter.SetActive(false);
                SetActiveRecursively(_femaleCharacter.transform, false);
                savePlayerData.GenderToggle(true);
            }
        }
        public void ToggleFemale()
        {
            if (femaleToggle.isOn == false)
            {

                maleToggle.isOn = true;
                _maleCharacter.SetActive(true);
                SetActiveRecursively(_maleCharacter.transform, true);
                _maleDisplayUsernameText.GetComponent<Text>().text = savePlayerData.usernameREF;

                _femaleCharacter.SetActive(false);
                SetActiveRecursively(_femaleCharacter.transform, false);
                savePlayerData.GenderToggle(true);

            }
            else if (femaleToggle.isOn == true)
            {

                maleToggle.isOn = false;
                _femaleCharacter.SetActive(true);
                SetActiveRecursively(_femaleCharacter.transform, true);
                _femaleDisplayUsernameText.GetComponent<Text>().text = savePlayerData.usernameREF;

                _maleCharacter.SetActive(false);
                SetActiveRecursively(_maleCharacter.transform, false);
                savePlayerData.GenderToggle(false);
            }
        }

        public void SetUsername(string name)
        {
            if (savePlayerData.maleREF)
            {
                if (name != "")
                {
                    _maleDisplayUsernameText.GetComponent<Text>().text = name;
                    Debug.Log(name);
                }
            }
            else
            {
                if (name != "")
                {
                    _femaleDisplayUsernameText.GetComponent<Text>().text = name;
                    Debug.Log(name);
                }
            }
            if (name != "")
            {
                savePlayerData.SaveUsername(name);
            }
        }

        public void dropdownREFComponent()
        {
            valueDropdown.value = GameObject.Find("SwordDropdown").GetComponent<Dropdown>().value;
            SetSwordChoice(valueDropdown.value);
        }

        public void SetSwordChoice(int dropdownValue)
        {
            Debug.Log(dropdownValue);
            if (dropdownValue == 0) // Purple Sword & Yellow Shield
            {
                swordPurple = true;
                shieldWood = false;
                updatePlayerINFO();

            }
            else if (dropdownValue == 1) // Purple Sword & Wooden Shield
            {
                swordPurple = true;
                shieldWood = true;
                updatePlayerINFO();

            }
            else if (dropdownValue == 2) // Green Sword & Yellow Shield
            {
                swordPurple = false;
                shieldWood = false;
                updatePlayerINFO();

            }
            else if (dropdownValue == 3) // Green Sword & Wooden Shield
            {
                swordPurple = false;
                shieldWood = true;
                updatePlayerINFO();
            }
        }


        public void updatePlayerINFO()
        {
            savePlayerData.shieldWoodREF = shieldWood;
            savePlayerData.swordPurpleREF = swordPurple;

            if (maleToggle.isOn)
            {
                // _maleCharacter.SetActive(true);

                // _femaleCharacter.SetActive(false);
                // SetActiveRecursively(_femaleCharacter.transform, false);

                if (shieldWood)
                {
                    Debug.Log("Wooden Shield...");
                    if (playerYellowShieldFEMALE != null)
                    {
                        playerYellowShieldMALE.SetActive(false);
                    }
                    if (playerWoodenShieldFEMALE != null)
                    {
                        playerWoodenShieldMALE.SetActive(true);
                    }

                    if (playerWoodenShieldFEMALE != null)
                    {
                        playerWoodenShieldFEMALE.SetActive(false);
                    }

                    if (playerYellowShieldFEMALE != null)
                    {
                        playerYellowShieldFEMALE.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Yellow Shield...");
                    if (playerWoodenShieldMALE != null)
                    {
                        playerWoodenShieldMALE.SetActive(false);
                    }

                    if (playerYellowShieldMALE != null)
                    {
                        playerYellowShieldMALE.SetActive(true);
                    }

                    if (playerWoodenShieldFEMALE != null)
                    {
                        playerWoodenShieldFEMALE.SetActive(false);
                    }

                    if (playerYellowShieldFEMALE != null)
                    {
                        playerYellowShieldFEMALE.SetActive(false);
                    }
                }
                // Get current saved sword type if true...
                if (swordPurple)
                {
                    Debug.Log("Purple Sword...");
                    if (playerGreenSwordFEMALE != null)
                    {
                        playerGreenSwordFEMALE.SetActive(false);
                    }
                    if (playerPurpleSwordFEMALE != null)
                    {
                        playerPurpleSwordFEMALE.SetActive(true);
                    }

                    if (playerPurpleSwordMALE != null)
                    {
                        playerPurpleSwordMALE.SetActive(false);
                    }

                    if (playerGreenSwordMALE != null)
                    {
                        playerGreenSwordMALE.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Green Sword...");
                    if (playerPurpleSwordFEMALE != null)
                    {
                        playerPurpleSwordFEMALE.SetActive(false);
                    }

                    if (playerGreenSwordFEMALE != null)
                    {
                        playerGreenSwordFEMALE.SetActive(true);
                    }

                    if (playerWoodenShieldMALE)
                    {
                        playerWoodenShieldMALE.SetActive(false);
                    }

                    if (playerYellowShieldMALE)
                    {
                        playerYellowShieldMALE.SetActive(false);
                    }
                    // playerYellowSwordFEMALE.SetActive(false);
                }
            }
            else
            {
                // _femaleCharacter.SetActive(true);

                // _maleCharacter.SetActive(false);
                // SetActiveRecursively(_maleCharacter.transform, false);

                if (shieldWood)
                {
                    Debug.Log("Wooden Shield...");
                    playerYellowShieldFEMALE.SetActive(false);
                    playerWoodenShieldFEMALE.SetActive(true);

                    if (playerWoodenShieldMALE != null)
                    {
                        playerWoodenShieldMALE.SetActive(false);
                    }

                    if (playerYellowShieldMALE != null)
                    {
                        playerYellowShieldMALE.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Yellow Shield...");
                    playerWoodenShieldFEMALE.SetActive(false);
                    playerYellowShieldFEMALE.SetActive(true);

                    playerWoodenShieldMALE.SetActive(false);
                    playerYellowShieldMALE.SetActive(false);
                }
            }
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