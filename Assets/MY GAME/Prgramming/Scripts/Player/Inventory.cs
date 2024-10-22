using GameDev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


namespace Player
{
    [AddComponentMenu("GameDev/Player/Inventory")]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] GameObject menuINV;
        [SerializeField] GameObject profileBTN;
        [SerializeField] GameObject forwardBTN;
        [SerializeField] GameObject playerHUD;
        [SerializeField] GameObject pausedBTN;
        GameObject _player;

        [SerializeField] bool _inINV = false;

        [SerializeField] GameObject invMenuFirstObject;

        private void Start()
        {
            _inINV = false;
            menuINV.SetActive(_inINV);

            _player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            if (GameManager.instance.state == GameStates.Play)
            {
                if (!_inINV && Input.GetButtonDown("Inventory"))
                {
                    OpenINV();
                }
            }  
            else if (GameManager.instance.state == GameStates.Menu)
            {
                if (_inINV && Input.GetButtonDown("Inventory"))
                {
                    CloseINV();
                }
            }
        }

        public void SelectObjectUI(GameObject firstObject)
        {
            // Clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            // Set a new selected object
            EventSystem.current.SetSelectedGameObject(firstObject);
        }

        public void OpenINV()
        {
            SelectObjectUI(invMenuFirstObject);

            GameManager.instance.OnMenu();
            _inINV = true;
            GameManager.instance.inINV = _inINV;

            menuINV.SetActive(_inINV);
            playerHUD.SetActive(false);
            pausedBTN.SetActive(false);
            profileBTN.SetActive(false);
        }

        public void CloseINV()
        {
            CheckState();
            _inINV = false;
            menuINV.SetActive(_inINV);
            playerHUD.SetActive(true);
            pausedBTN.SetActive(true);
            profileBTN.SetActive(true);

            SelectObjectUI(forwardBTN);
        }

        private void CheckState()
        {
            GameManager.instance.inINV = false;
            GameManager.instance.OnPlay();
        }
    }
}