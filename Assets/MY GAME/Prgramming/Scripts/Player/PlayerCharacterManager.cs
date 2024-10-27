using GameDev;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterManager : MonoBehaviour
{
    public static PlayerCharacterManager Instance;

    public SavePlayerData _playerData;

    public Text username;
    public bool male;
    public bool shield;
    public Text level;
    public bool sword;


    [Header("Male Player Objects")]
    public GameObject _malePlayer;
    public GameObject _maleModel;
    public GameObject playerWoodenShieldMALE;
    public GameObject playerYellowShieldMALE;
    public GameObject playerPurpleSwordMALE;
    public GameObject playerGreenSwordMALE;


    [Header("Female Player Objects")]
    public GameObject _femalePlayer;
    public GameObject _femaleModel;
    public GameObject playerWoodenShieldFEMALE;
    public GameObject playerYellowShieldFEMALE;
    public GameObject playerPurpleSwordFEMALE;
    public GameObject playerGreenSwordFEMALE;

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


        username = GameObject.Find("Username").GetComponent<Text>();
        level = GameObject.Find("characterLevel").GetComponent<Text>();



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
    }






    public void updatePlayerINFO()
    {
        male = _playerData.maleREF;
        shield = _playerData.shieldWoodREF;
        sword = _playerData.swordPurpleREF;

        if (male)
        {
            _malePlayer.SetActive(true);
            _maleModel.SetActive(true);

            _femalePlayer.SetActive(false);
            _femaleModel.SetActive(false);
            SetActiveRecursively(_femalePlayer.transform, false);

            if (shield)
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
                playerWoodenShieldMALE.SetActive(false);
                playerYellowShieldMALE.SetActive(true);

                playerWoodenShieldFEMALE.SetActive(false);
                playerYellowShieldFEMALE.SetActive(false);
            }
            // Get current saved sword type if true...
            if (sword)
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
                playerPurpleSwordFEMALE.SetActive(false);
                playerGreenSwordFEMALE.SetActive(true);

                playerWoodenShieldMALE.SetActive(false);
                playerYellowShieldMALE.SetActive(false);
                // playerYellowSwordFEMALE.SetActive(false);
            }
        }
        else
        {
            _femalePlayer.SetActive(true);
            _femaleModel.SetActive(true);

            _malePlayer.SetActive(false);
            _maleModel.SetActive(false);
            SetActiveRecursively(_malePlayer.transform, false);

            if (shield)
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

    public void PlayerINFO()
    {
        username.text = _playerData.usernameREF;

        level.text = _playerData.levelREF.ToString();

        Debug.Log($"{_playerData.usernameREF}: {_playerData.maleREF}: {_playerData.levelREF}: {_playerData.swordPurpleREF}: {_playerData.shieldWoodREF}");
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
