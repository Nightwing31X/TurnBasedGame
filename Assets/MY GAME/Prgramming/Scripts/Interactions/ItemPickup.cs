using System.Collections;
using System.Collections.Generic;
using GameDev;
using Interactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    private int _currentValue;
    private int _maxValue;

    SavePlayerData _savePlayerData;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_savePlayerData == null)
            {
                _savePlayerData = GameObject.Find("PlayerManager").GetComponent<SavePlayerData>();
            }

            _currentValue = _savePlayerData.currentBagValueREF;
            _maxValue = _savePlayerData.maxBagValueREF;
            if (_currentValue != _maxValue)
            {
                ChoicePickUp(item);
            }
            else
            {
                Debug.Log("Bag is full... " + _currentValue);
            }
        }
    }

    public void ChoicePickUp(Item itemREF)
    {
        InventoryManager.Instance.NameItemDetailText.text = item.name;
        InventoryManager.Instance.DescriptionItemDetailText.text = item.description;
        InventoryManager.Instance.IconItemDetail.texture = item.artwork;
        InventoryManager.Instance.promptChoice.SetActive(true);
        InventoryManager.Instance.PickUpItemREF = itemREF;
        InventoryManager.Instance.ItemOnGround = gameObject;
    }

    // public void PutInBag()
    // {
    //     InventoryManager.Instance.promptChoice.SetActive(false);
    //     InventoryManager.Instance.Add(item);
    //     Destroy(gameObject);
    // }
}
