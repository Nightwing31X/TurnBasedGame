using GameDev;
using Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
        // [SerializeField] Toggle enableRemoveToggle;

        [SerializeField] bool _inINV = false;

        [SerializeField] GameObject invMenuFirstObject;
        public Text title;
        public Text description;

        GameObject _playerManager;


        private void Start()
        {
            _inINV = false;
            menuINV.SetActive(_inINV);

            _playerManager = GameObject.Find("PlayerManager");
        }

        void Update()
        {
            if (GameManager.instance.state == GameStates.PlayerTurn)
            {
                if (!_inINV && Input.GetButtonDown("Inventory"))
                {
                    OpenINV();
                    InventoryManager.Instance.ListItems();
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
            title.text = "Name of the Item";
            description.text = "Description about the item...";

            // SelectObjectUI(invMenuFirstObject);

            GameManager.instance.OnMenu();
            _inINV = true;
            GameManager.instance.inINV = _inINV;

            PlayerCharacterManager.Instance.PlayerINFO();
            _playerManager.GetComponent<Health>().UpdatePlayersHealth(true);

            pausedBTN.SetActive(false);
            profileBTN.SetActive(false);

            menuINV.SetActive(_inINV);
            menuINV.transform.Find("INVBackground").GetComponent<Animator>().SetBool("INVBackgroundFadeIn", true);
            menuINV.transform.Find("BagObject").GetComponent<Animator>().SetBool("BagObjectOpen", true);
            menuINV.transform.Find("PlayerINV").GetComponent<Animator>().SetBool("PlayerINVOpen", true);
            menuINV.transform.Find("PlayerInfo").GetComponent<Animator>().SetBool("PlayerInfoOpen", true);

            // playerHUD.SetActive(false);
        }


        public void CloseINV()
        {
            title.text = "Name of the Item";
            description.text = "Description about the item...";
            CheckState();
            StartCoroutine(ANIMCloseINV());


            // enableRemoveToggle.isOn = false;
            SelectObjectUI(forwardBTN);
        }
        IEnumerator ANIMCloseINV()
        {
            menuINV.transform.Find("INVBackground").GetComponent<Animator>().SetBool("INVBackgroundFadeIn", false);
            menuINV.transform.Find("BagObject").GetComponent<Animator>().SetBool("BagObjectOpen", false);
            menuINV.transform.Find("PlayerINV").GetComponent<Animator>().SetBool("PlayerINVOpen", false);
            menuINV.transform.Find("PlayerInfo").GetComponent<Animator>().SetBool("PlayerInfoOpen", false);
            yield return new WaitForSeconds(1f);
            _inINV = false;
            menuINV.SetActive(_inINV);
            playerHUD.SetActive(true);
            pausedBTN.SetActive(true);
            profileBTN.SetActive(true);
            InventoryManager.Instance.clearItemsOnClose();
        }

        private void CheckState()
        {
            GameManager.instance.inINV = false;
            GameManager.instance.OnPlayerTurn();
        }
    }
}