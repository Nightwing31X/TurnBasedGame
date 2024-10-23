using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public Toggle EnableRemove;

        public InventoryItemController[] InventoryItems;

        // Text Elements from the scriptableObject - I reference them in the code...
        [HideInInspector] public RawImage itemIcon;
        [HideInInspector] public Button removeButton;
        [HideInInspector] public Text BagTitleText;

        public int currentItemValue = 0;
        public int maxNumber = 24;


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
        }

        public void Add(Item item)
        {
            Items.Add(item);
            currentItemValue = currentItemValue + 1;
        }

        public void Remove(Item item)
        {
            currentItemValue = currentItemValue - 1;
            Items.Remove(item);
            checkItemNumber();
        }

        public void ListItems()
        {
            clearItemsOnClose();
            foreach (var item in Items)
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                // var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
                itemIcon = obj.transform.Find("Item/ItemIcon").GetComponent<RawImage>();
                removeButton = obj.transform.Find("Item/RemoveButton").GetComponent<Button>();
                BagTitleText = GameObject.Find("BagTitleText").GetComponent<Text>();

                checkItemNumber();
                itemIcon.texture = item.artwork;
                // itemIcon.texture = item.artwork;

                if (EnableRemove.isOn)
                {
                    removeButton.gameObject.SetActive(true);
                }
            }

            SetInventoryItems();
        }

        public void checkItemNumber()
        {
            BagTitleText.text = $"Bag - {currentItemValue}/{maxNumber}";
        }

        public void EnableItemsRemove()
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("Item/RemoveButton").gameObject.SetActive(EnableRemove.isOn);
            }
            // checkItemNumber();
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
            checkItemNumber();
        }
    }
}
