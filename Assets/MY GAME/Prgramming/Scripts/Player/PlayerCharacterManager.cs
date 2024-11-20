using GameDev;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterManager : MonoBehaviour
{
    public static PlayerCharacterManager Instance;

    public SavePlayerData _playerData;

    public Text username;
    public bool male;
    public bool shieldWood;
    public Text level;
    public bool swordPurple;


    [Header("Male Player Objects")]
    public GameObject _malePlayer;
    public GameObject _maleModel;
    public GameObject playerWoodenShieldMALE;
    public GameObject playerYellowShieldMALE;
    public GameObject playerPurpleSwordMALE;
    public GameObject playerGreenSwordMALE;
    public GameObject modelWoodenShieldMALE;
    public GameObject modelYellowShieldMALE;
    public GameObject modelPurpleSwordMALE;
    public GameObject modelGreenSwordMALE;


    [Header("Female Player Objects")]
    public GameObject _femalePlayer;
    public GameObject _femaleModel;
    public GameObject playerWoodenShieldFEMALE;
    public GameObject playerYellowShieldFEMALE;
    public GameObject playerPurpleSwordFEMALE;
    public GameObject playerGreenSwordFEMALE;
    public GameObject modelWoodenShieldFEMALE;
    public GameObject modelYellowShieldFEMALE;
    public GameObject modelPurpleSwordFEMALE;
    public GameObject modelGreenSwordFEMALE;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

        _playerData = GetComponent<SavePlayerData>();
        _malePlayer = GameObject.Find("MalePlayer");
        _maleModel = GameObject.Find("MaleModel");

        _femalePlayer = GameObject.Find("FemalePlayer");
        _femaleModel = GameObject.Find("FemaleModel");


        // username = GameObject.Find("Username").GetComponent<Text>();
        // level = GameObject.Find("characterLevel").GetComponent<Text>();

        // Male Objects - Shield
        playerWoodenShieldMALE = GameObject.Find("INVWoodenShieldMALE");
        playerYellowShieldMALE = GameObject.Find("INVYellowShieldMALE");
        modelWoodenShieldMALE = GameObject.Find("modelWoodenShieldMALE");
        modelYellowShieldMALE = GameObject.Find("modelYellowShieldMALE");
        // Male Objects - Sword
        playerPurpleSwordMALE = GameObject.Find("INVPurpleSwordMALE");
        playerGreenSwordMALE = GameObject.Find("INVGreenSwordMALE");
        modelPurpleSwordMALE = GameObject.Find("modelPurpleSwordMALE");
        modelGreenSwordMALE = GameObject.Find("modelGreenSwordMALE");

        // Female Objects - Shield
        playerWoodenShieldFEMALE = GameObject.Find("INVWoodenShieldFEMALE");
        playerYellowShieldFEMALE = GameObject.Find("INVYellowShieldFEMALE");
        modelWoodenShieldFEMALE = GameObject.Find("modelWoodenShieldFEMALE");
        modelYellowShieldFEMALE = GameObject.Find("modelYellowShieldFEMALE");
        // Female Objects - Sword
        playerPurpleSwordFEMALE = GameObject.Find("INVPurpleSwordFEMALE");
        playerGreenSwordFEMALE = GameObject.Find("INVGreenSwordFEMALE");
        modelPurpleSwordFEMALE = GameObject.Find("modelPurpleSwordFEMALE");
        modelGreenSwordFEMALE = GameObject.Find("modelGreenSwordFEMALE");
    }






    public void updatePlayerINFO()
    {
        male = _playerData.maleREF;
        // _playerData.shieldWoodREF = shieldWood;
        shieldWood = _playerData.shieldWoodREF;
        Debug.Log(shieldWood);
        // _playerData.swordPurpleREF = swordPurple;
        swordPurple = _playerData.swordPurpleREF;


        if (male)
        {
            _malePlayer.SetActive(true);
            _maleModel.SetActive(true);

            _femalePlayer.SetActive(false);
            _femaleModel.SetActive(false);
            SetActiveRecursively(_femalePlayer.transform, false);

            if (shieldWood)
            {
                Debug.Log("Wooden Shield... MALE");
                if (playerYellowShieldMALE != null)
                {
                    playerYellowShieldMALE.SetActive(false);
                    modelYellowShieldMALE.SetActive(false);
                }
                if (playerWoodenShieldMALE != null)
                {
                    playerWoodenShieldMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelWoodenShieldMALE.SetActive(true);
                }

                if (playerWoodenShieldFEMALE != null)
                {
                    playerWoodenShieldFEMALE.SetActive(false);
                    modelWoodenShieldFEMALE.SetActive(false);
                }

                if (playerYellowShieldFEMALE != null)
                {
                    playerYellowShieldFEMALE.SetActive(false);
                    modelYellowShieldFEMALE.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Yellow Shield... MALE");
                if (playerWoodenShieldMALE != null)
                {
                    playerWoodenShieldMALE.SetActive(false);
                    modelWoodenShieldMALE.SetActive(false);
                }
                if (playerYellowShieldMALE != null)
                {
                    playerYellowShieldMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelYellowShieldMALE.SetActive(true);
                }
                if (playerWoodenShieldFEMALE != null)
                {
                    playerWoodenShieldFEMALE.SetActive(false);
                    modelWoodenShieldFEMALE.SetActive(false);
                }
                if (playerYellowShieldFEMALE != null)
                {
                    playerYellowShieldFEMALE.SetActive(false);
                    modelYellowShieldFEMALE.SetActive(false);
                }
            }
            // Get current saved sword type if true...
            if (swordPurple)
            {
                Debug.Log("Purple Sword... MALE");
                if (playerGreenSwordMALE != null)
                {
                    playerGreenSwordMALE.SetActive(false);
                    modelGreenSwordMALE.SetActive(false);
                }
                if (playerPurpleSwordMALE != null)
                {
                    playerPurpleSwordMALE.SetActive(true);  //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelPurpleSwordMALE.SetActive(true);
                }

                if (playerPurpleSwordFEMALE != null)
                {
                    playerPurpleSwordFEMALE.SetActive(false);
                    modelPurpleSwordFEMALE.SetActive(false);
                }

                if (playerGreenSwordFEMALE != null)
                {
                    playerGreenSwordFEMALE.SetActive(false);
                    modelGreenSwordFEMALE.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Green Sword... MALE");
                if (playerPurpleSwordMALE != null)
                {
                    playerPurpleSwordMALE.SetActive(false);
                    modelPurpleSwordMALE.SetActive(false);
                }
                if (playerGreenSwordMALE != null)
                {
                    playerGreenSwordMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelGreenSwordMALE.SetActive(true);
                }
                if (playerYellowShieldFEMALE != null)
                {
                    playerYellowShieldFEMALE.SetActive(false);
                    modelYellowShieldFEMALE.SetActive(false);
                }
                if (playerWoodenShieldFEMALE != null)
                {
                    playerWoodenShieldFEMALE.SetActive(false);
                    modelWoodenShieldFEMALE.SetActive(false);
                }
            }
        }
        else
        {
            _femalePlayer.SetActive(true);
            _femaleModel.SetActive(true);

            _malePlayer.SetActive(false);
            _maleModel.SetActive(false);
            SetActiveRecursively(_malePlayer.transform, false);

            if (shieldWood)
            {
                Debug.Log("Wooden Shield... FEMALE");
                if (playerYellowShieldFEMALE != null)
                {
                    playerYellowShieldFEMALE.SetActive(false);
                    modelYellowShieldFEMALE.SetActive(false);
                }
                if (playerWoodenShieldFEMALE != null)
                {
                    playerWoodenShieldFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelWoodenShieldFEMALE.SetActive(true);

                }
                if (playerWoodenShieldMALE != null)
                {
                    playerWoodenShieldMALE.SetActive(false);
                    modelWoodenShieldMALE.SetActive(false);
                }
                if (playerYellowShieldMALE != null)
                {
                    playerYellowShieldMALE.SetActive(false);
                    modelYellowShieldMALE.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Yellow Shield... FEMALE");
                if (playerWoodenShieldFEMALE != null)
                {
                    playerWoodenShieldFEMALE.SetActive(false);
                    modelWoodenShieldFEMALE.SetActive(false);
                }
                if (playerYellowShieldFEMALE != null)
                {
                    playerYellowShieldFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelYellowShieldFEMALE.SetActive(true);
                }
                if (playerWoodenShieldMALE != null)
                {
                    playerWoodenShieldMALE.SetActive(false);
                    modelWoodenShieldMALE.SetActive(false);
                }
                if (playerYellowShieldMALE != null)
                {
                    playerYellowShieldMALE.SetActive(false);
                    modelYellowShieldMALE.SetActive(false);
                }
            }
            if (swordPurple)
            {
                Debug.Log("Purple Sword... FEMALE");
                if (playerGreenSwordFEMALE != null)
                {
                    playerGreenSwordFEMALE.SetActive(false);
                    modelGreenSwordFEMALE.SetActive(false);
                }
                if (playerPurpleSwordFEMALE != null)
                {
                    playerPurpleSwordFEMALE.SetActive(true);  //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelPurpleSwordFEMALE.SetActive(true);
                }

                if (playerPurpleSwordMALE != null)
                {
                    playerPurpleSwordMALE.SetActive(false);
                    modelPurpleSwordMALE.SetActive(false);
                }

                if (playerGreenSwordMALE != null)
                {
                    playerGreenSwordMALE.SetActive(false);
                    modelGreenSwordMALE.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Green Sword... FEMALE");
                if (playerPurpleSwordFEMALE != null)
                {
                    playerPurpleSwordFEMALE.SetActive(false);
                    modelPurpleSwordFEMALE.SetActive(false);
                }
                if (playerGreenSwordFEMALE != null)
                {
                    playerGreenSwordFEMALE.SetActive(true); //* <------- THIS IS THE OBJECT WHICH TURNS ON
                    modelGreenSwordFEMALE.SetActive(true);
                }
                if (playerYellowShieldMALE != null)
                {
                    playerYellowShieldMALE.SetActive(false);
                    modelYellowShieldMALE.SetActive(false);
                }
                if (playerWoodenShieldMALE != null)
                {
                    playerWoodenShieldMALE.SetActive(false);
                    modelWoodenShieldMALE.SetActive(false);
                }
            }
        }
    }

    public void PlayerINFO()
    {
        // Debug.Log("Name and level....");
        username.text = _playerData.usernameREF;

        level.text = _playerData.levelREF.ToString();

        // Debug.Log($"{_playerData.usernameREF}: {_playerData.maleREF}: {_playerData.levelREF}: {_playerData.swordPurpleREF}: {_playerData.shieldWoodREF}");
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
}
