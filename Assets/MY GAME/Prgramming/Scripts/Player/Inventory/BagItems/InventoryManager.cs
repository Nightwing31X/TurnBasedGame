using System.Collections;
using System.Collections.Generic;
using GameDev;
using UnityEngine;
using UnityEngine.UI;

namespace Interactions
{
    [AddComponentMenu("GameDev/Player/Inventory Manager")]
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public List<Item> Items = new List<Item>();

        public Transform ItemContent; // Where the Items get filled...
        public GameObject InventoryItem; // 2D Items

        public int currentBagValue = 0;
        public int maxBagValue = 24;

        public Toggle EnableRemove;

        public InventoryItemController[] InventoryItems;

        // Text Elements from the scriptableObject - I reference them in the code...
        [HideInInspector] public Text bagTitle;
        [HideInInspector] public RawImage itemIcon;
        [HideInInspector] public Button removeButton;

        SavePlayerData _savePlayerData;

        [Header("Objects for the PickUp Prompt")]
        public GameObject promptChoice;
        public Text NameItemDetailText;
        public Text DescriptionItemDetailText;
        public RawImage IconItemDetail;

        public Item PickUpItemREF;
        public GameObject ItemOnGround;

        private void Awake()
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

            _savePlayerData = GameObject.Find("PlayerManager").GetComponent<SavePlayerData>();
            promptChoice.SetActive(false);
        }




        public void Add(Item item)
        {
            Items.Add(item);
            ChangeValue(1);
        }

        public void Remove(Item item)
        {
            ChangeValue(-1);
            checkValue();
            Items.Remove(item);
        }


        public void NoPickUp()
        {
            Debug.Log("Player choose not to pick up the item... Doesn't go away or enter the bag");
            promptChoice.GetComponentInChildren<Animator>().SetBool("Hide", true);
            promptChoice.GetComponentInChildren<Animator>().SetBool("Show", false);
            StartCoroutine(HidePromptChoice());
        }

        public void PutInBag()
        {
            promptChoice.GetComponentInChildren<Animator>().SetBool("Hide", true);
            promptChoice.GetComponentInChildren<Animator>().SetBool("Show", false);
            StartCoroutine(HidePromptChoice());
            Add(PickUpItemREF);
            Destroy(ItemOnGround);
            GameManager.instance.OnPlayerTurn();
        }

        IEnumerator HidePromptChoice()
        {
            yield return new WaitForSeconds(2f);
            promptChoice.SetActive(false);
        }

        public void ListItems()
        {
            clearItemsOnClose();
            foreach (var item in Items)
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                // var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
                bagTitle = GameObject.Find("BagTitleText").GetComponent<Text>();
                itemIcon = obj.transform.Find("ItemIcon").GetComponent<RawImage>();
                removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

                itemIcon.texture = item.artwork;
                // itemIcon.texture = item.artwork;

                if (EnableRemove.isOn)
                {
                    removeButton.gameObject.SetActive(true);
                }
            }

            SetInventoryItems();
        }


        public void ChangeValue(int num) //* Changes the Text CurrentValue to increase or decrease
        {
            currentBagValue = currentBagValue + num;
            _savePlayerData.currentBagValueREF = currentBagValue;
        }
        public void checkValue() //* Displays what every the change was above
        {
            if (bagTitle != null)
            {
                bagTitle.text = $"Bag - {currentBagValue}/{maxBagValue}";
            }
        }

        public void EnableItemsRemove()
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(EnableRemove.isOn);
            }
        }

        public void clearItemsOnClose()
        {
            // Clean content before open.  
            if (ItemContent == null) return;

            foreach (Transform item in ItemContent)
            {
                Destroy(item.gameObject);
            }
        }

        public void SetInventoryItems()
        {
            InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>(); // So we can know what item is saved

            for (int i = 0; i < Mathf.Min(Items.Count, InventoryItems.Length); i++)
            {
                InventoryItems[i].AddItem(Items[i]); // Calls the function from InventoryItemController to "save" the item.
            }
            checkValue();
        }
    }
}
