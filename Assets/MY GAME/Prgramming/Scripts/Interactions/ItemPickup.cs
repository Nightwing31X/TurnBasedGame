using System.Collections;
using System.Collections.Generic;
using GameDev;
using Interactions;
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

            if (item.isPotion)
            { 
                gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                gameObject.GetComponentInChildren<Animator>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                gameObject.GetComponent<Animator>().enabled = false;
            }
        }
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        gameObject.GetComponent<MeshRenderer>().enabled = false;
    //        gameObject.GetComponent<Animator>().enabled = false;
    //    }
    //}
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (item.isPotion)
            {
                gameObject.GetComponentInChildren<ParticleSystem>().Play();
                gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
                gameObject.GetComponentInChildren<Animator>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<Animator>().enabled = true;
            }
        }
    }

    public void ChoicePickUp(Item itemREF)
    {
        InventoryManager.Instance.NameItemDetailText.text = item.name;
        InventoryManager.Instance.DescriptionItemDetailText.text = item.description;
        InventoryManager.Instance.IconItemDetail.texture = item.artwork;

        InventoryManager.Instance.promptChoice.SetActive(true);
        InventoryManager.Instance.promptChoice.GetComponentInChildren<Animator>().SetBool("Show", true);
        InventoryManager.Instance.promptChoice.GetComponentInChildren<Animator>().SetBool("Hide", false);


        InventoryManager.Instance.PickUpItemREF = itemREF;
        InventoryManager.Instance.ItemOnGround = gameObject;
    }
}
