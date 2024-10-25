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
    public Text level;


    public GameObject _malePlayer;
    public GameObject _maleModel;
    public GameObject _femalePlayer;
    public GameObject _femaleModel;

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
    }
    public void updatePlayerINFO()
    {
        male = _playerData.maleREF;

        if (male)
        {
            _malePlayer.SetActive(true);
            _maleModel.SetActive(true);

            _femalePlayer.SetActive(false);
            _femaleModel.SetActive(false);
            SetActiveRecursively(_femalePlayer.transform, false);
        }
        else
        {
            _femalePlayer.SetActive(true);
            _femaleModel.SetActive(true);

            _malePlayer.SetActive(false);
            _maleModel.SetActive(false);

            SetActiveRecursively(_malePlayer.transform, false);
        }
        //StartCoroutine(SendPlayerData());
    }

    //IEnumerator SendPlayerData()
    //{
    //    yield return new WaitForSeconds(0.01f);
    //    male = _playerData.maleREF;

    //    if (male)
    //    {
    //        _malePlayer.SetActive(true);
    //        _maleModel.SetActive(true);

    //        _femalePlayer.SetActive(false);
    //        _femaleModel.SetActive(false);
    //        SetActiveRecursively(_femalePlayer.transform, false);
    //    }
    //    else
    //    {
    //        _femalePlayer.SetActive(true);
    //        _femaleModel.SetActive(true);

    //        _malePlayer.SetActive(false);
    //        _maleModel.SetActive(false);

    //        SetActiveRecursively(_malePlayer.transform, false);
    //    }
    //}

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
