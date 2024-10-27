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
        [SerializeField] private GameObject playerWoodenShieldMALE;
        [SerializeField] private GameObject playerYellowShieldMALE;
        [SerializeField] private GameObject playerPurpleSwordMALE;
        [SerializeField] private GameObject playerGreenSwordMALE;
        [Header("Female Player Objects")]
        [SerializeField] private GameObject playerWoodenShieldFEMALE;
        [SerializeField] private GameObject playerYellowShieldFEMALE;
        [SerializeField] private GameObject playerPurpleSwordFEMALE;
        [SerializeField] private GameObject playerGreenSwordFEMALE;
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
            playerWoodenShieldMALE = GameObject.Find("playerWoodenShieldMALE");
            playerYellowShieldMALE = GameObject.Find("playerYellowShieldMALE");
            // Male Objects - Sword
            playerPurpleSwordMALE = GameObject.Find("playerPurpleSwordMALE");
            playerGreenSwordMALE = GameObject.Find("playerGreenSwordMALE");

            // Female Objects - Shield
            playerWoodenShieldFEMALE = GameObject.Find("playerWoodenShieldFEMALE");
            playerYellowShieldFEMALE = GameObject.Find("playerYellowShieldFEMALE");
            // Female Objects - Sword
            playerGreenSwordFEMALE = GameObject.Find("playerGreenSwordFEMALE");
            playerPurpleSwordFEMALE = GameObject.Find("playerPurpleSwordFEMALE");

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

            SetSwordChoice(0);
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
            if (savePlayerData.firstTimeREF)
            {
                valueDropdown = GameObject.Find("SwordDropdown").GetComponent<Dropdown>();
                SetSwordChoice(valueDropdown.value);
            }
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
                    Debug.Log("Wooden Shield... MALE");
                    if (playerYellowShieldMALE != null)
                    {
                        playerYellowShieldMALE.SetActive(false);
                    }
                    if (playerWoodenShieldMALE != null)
                    {
                        playerWoodenShieldMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
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
                    Debug.Log("Yellow Shield... MALE");
                    if (playerWoodenShieldMALE != null)
                    {
                        playerWoodenShieldMALE.SetActive(false);
                    }

                    if (playerYellowShieldMALE != null)
                    {
                        playerYellowShieldMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
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
                    Debug.Log("Purple Sword... MALE");
                    if (playerGreenSwordMALE != null)
                    {
                        playerGreenSwordMALE.SetActive(false);
                    }
                    if (playerPurpleSwordMALE != null)
                    {
                        playerPurpleSwordMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    }

                    if (playerPurpleSwordFEMALE != null)
                    {
                        playerPurpleSwordFEMALE.SetActive(false);
                    }

                    if (playerGreenSwordFEMALE != null)
                    {
                        playerGreenSwordFEMALE.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("Green Sword... MALE");
                    if (playerPurpleSwordMALE != null)
                    {
                        playerPurpleSwordMALE.SetActive(false);
                    }

                    if (playerGreenSwordMALE != null)
                    {
                        playerGreenSwordMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    }

                    if (playerPurpleSwordFEMALE != null)
                    {
                        playerPurpleSwordFEMALE.SetActive(false);
                    }

                    if (playerGreenSwordFEMALE != null)
                    {
                        playerGreenSwordFEMALE.SetActive(false);
                    }
                }
            }
            else
            {
                // _femaleCharacter.SetActive(true);

                // _maleCharacter.SetActive(false);
                // SetActiveRecursively(_maleCharacter.transform, false);

#region New test Idea for the female
                if (shieldWood)
                {
                    Debug.Log("Wooden Shield... FEMALE");
                    if (playerYellowShieldFEMALE != null)
                    {
                        playerYellowShieldFEMALE.SetActive(false);
                    }
                    if (playerWoodenShieldFEMALE != null)
                    {
                        playerWoodenShieldFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    }

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
                    Debug.Log("Yellow Shield... FEMALE");
                    if (playerWoodenShieldFEMALE != null)
                    {
                        playerWoodenShieldFEMALE.SetActive(false);
                    }

                    if (playerYellowShieldFEMALE != null)
                    {
                        playerYellowShieldFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    }

                    if (playerWoodenShieldMALE != null)
                    {
                        playerWoodenShieldMALE.SetActive(false);
                    }

                    if (playerYellowShieldMALE != null)
                    {
                        playerYellowShieldMALE.SetActive(false);
                    }
                }
                // Get current saved sword type if true...
                if (swordPurple)
                {
                    Debug.Log("Purple Sword... FEMALE");
                    if (playerGreenSwordFEMALE != null)
                    {
                        playerGreenSwordFEMALE.SetActive(false);
                    }
                    if (playerPurpleSwordFEMALE != null)
                    {
                        playerPurpleSwordFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
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
                    Debug.Log("Green Sword... FEMALE");
                    if (playerPurpleSwordFEMALE != null)
                    {
                        playerPurpleSwordFEMALE.SetActive(false);
                    }

                    if (playerGreenSwordFEMALE != null)
                    {
                        playerGreenSwordFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
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
#endregion


                // if (shieldWood)
                // {
                //     Debug.Log("Wooden Shield... FEMALE");
                //     if (playerYellowShieldFEMALE != null)
                //     {
                //         playerYellowShieldFEMALE.SetActive(false);
                //     }
                //     if (playerWoodenShieldFEMALE != null)
                //     {
                //         playerWoodenShieldFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON                    
                //     }
                //     if (playerWoodenShieldMALE != null)
                //     {
                //         playerWoodenShieldMALE.SetActive(false);
                //     }
                //     if (playerYellowShieldMALE != null)
                //     {
                //         playerYellowShieldMALE.SetActive(false);
                //     }
                // }
                // else
                // {
                //     Debug.Log("Yellow Shield... FEMALE");
                //     if (playerWoodenShieldFEMALE != null)
                //     {
                //         playerWoodenShieldFEMALE.SetActive(false);
                //     }
                //     if (playerYellowShieldFEMALE != null)
                //     {
                //         playerYellowShieldFEMALE.SetActive(true);  //* <------- THIS IS THE OBJECT WHICH TURNS ON
                //     }
                //     if (playerWoodenShieldMALE != null)
                //     {
                //         playerWoodenShieldMALE.SetActive(false);
                //     }
                //     if (playerYellowShieldMALE != null)
                //     {
                //         playerYellowShieldMALE.SetActive(false);
                //     }
                // }

                // if (swordPurple)
                // {
                //     Debug.Log("Purple Sword... FEMALE");
                //     if (playerGreenSwordFEMALE != null)
                //     {
                //         playerGreenSwordFEMALE.SetActive(false);
                //     }
                //     if (playerPurpleSwordFEMALE != null)
                //     {
                //         playerPurpleSwordFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                //     }

                //     if (playerPurpleSwordMALE != null)
                //     {
                //         playerPurpleSwordMALE.SetActive(false);
                //     }

                //     if (playerGreenSwordMALE != null)
                //     {
                //         playerGreenSwordMALE.SetActive(false);
                //     }
                // }
                // else
                // {
                //     Debug.Log("Green Sword... FEMALE");
                //     if (playerPurpleSwordFEMALE != null)
                //     {
                //         Debug.Log("I do run???");
                //         playerPurpleSwordFEMALE.SetActive(false);
                //         Debug.Log("I Should run!!!");
                //     }

                //     if (playerGreenSwordFEMALE != null)
                //     {
                //         playerGreenSwordFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                //     }

                //     if (playerPurpleSwordMALE != null)
                //     {
                //         playerPurpleSwordMALE.SetActive(false);
                //     }

                //     if (playerGreenSwordMALE != null)
                //     {
                //         playerGreenSwordMALE.SetActive(false);
                //     }
                // }
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