using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Interactions
{
    [AddComponentMenu("GameDev/Player/Bag Manager")]
    public class BagManager : MonoBehaviour
    {
        public static BagManager Instance;

        [Header("Bag Slots")]
        public List<Item> bagItems = new List<Item>();
        public Transform ItemBag; // Where the Items get filled...
        public GameObject BagSlot; // 2D Items
        public int currentBagItemValue = 0;
        public int maxBagNumber = 24;
        public bool isBagFull;

        // [Header("Player's Inventory Slots")]
        // public List<Item> ItemsINV = new List<Item>();
        // public Transform ItemPlayerINV;
        // public GameObject INVSlot;
        // public int currentINVItemValue = 0;
        // public int maxINVNumber = 8;

        public Toggle EnableRemove;

        public BagItemController[] InventoryItems;

        // Text Elements from the scriptableObject - I reference them in the code...
        [HideInInspector] public RawImage itemIcon;
        [HideInInspector] public Button removeButton;
        [HideInInspector] public Text BagTitleText;



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

            // StartINV();
        }

        // public void StartINV()
        // {
        //     foreach (var item in ItemsINV)
        //     {
        //         GameObject obj = Instantiate(INVSlot, ItemPlayerINV);
        //         // var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
        //         itemIcon = obj.transform.Find("Item/ItemIcon").GetComponent<RawImage>();
        //         removeButton = obj.transform.Find("Item/RemoveButton").GetComponent<Button>();
        //         BagTitleText = GameObject.Find("BagTitleText").GetComponent<Text>();

        //         checkItemNumber();
        //         itemIcon.texture = item.artwork;

        //         if (EnableRemove.isOn)
        //         {
        //             removeButton.gameObject.SetActive(true);
        //         }

        //         // if (item.isWeapon)
        //         // {
        //         //     Debug.Log($"I am a weapon: {item.name}");
        //         // }
        //     }
        // }

        public void Add(Item item)
        {
            bagItems.Add(item);
            currentBagItemValue = currentBagItemValue + 1;
        }

        public void Remove(Item item)
        {
            currentBagItemValue = currentBagItemValue - 1;
            bagItems.Remove(item);
            checkItemNumber();
        }

        public void ListItems()
        {
            clearItemsOnClose();
            foreach (var bagItem in bagItems)
            {
                GameObject obj = Instantiate(BagSlot, ItemBag);
                // var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
                itemIcon = obj.transform.Find("Item/ItemIcon").GetComponent<RawImage>();
                removeButton = obj.transform.Find("Item/RemoveButton").GetComponent<Button>();
                BagTitleText = GameObject.Find("BagTitleText").GetComponent<Text>();

                checkItemNumber();
                itemIcon.texture = bagItem.artwork;

                if (EnableRemove.isOn)
                {
                    removeButton.gameObject.SetActive(true);
                }

                // if (item.isWeapon)
                // {
                //     Debug.Log($"I am a weapon: {item.name}");
                // }
            }

            SetInventoryItems();
        }

        public void checkItemNumber()
        {
            if (BagTitleText != null)
            {
                BagTitleText.text = $"Bag - {currentBagItemValue}/{maxBagNumber}";
            }
        }

        public void EnableItemsRemove()
        {
            foreach (Transform item in ItemBag)
            {
                item.Find("Item/RemoveButton").gameObject.SetActive(EnableRemove.isOn);
            }
            // checkItemNumber();
        }


        public void clearItemsOnClose()
        {
            // Clean content before open.  
            if (ItemBag == null) return;

            foreach (Transform item in ItemBag)
            {
                Destroy(item.gameObject);
            }
        }

        public void SetInventoryItems()
        {
            InventoryItems = ItemBag.GetComponentsInChildren<BagItemController>(); // So we can know what item is saved

            for (int i = 0; i < Mathf.Min(bagItems.Count, InventoryItems.Length); i++)
            {
                InventoryItems[i].AddItem(bagItems[i]); // Calls the function from BagItemController to "save" the item.
            }
            checkItemNumber();
        }
    }
}
