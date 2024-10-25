using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    private int _currentValue;
    private int _maxValue;

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Debug.Log("Player hit...");
            _interact();
        }
    }

    public void _interact()
    {
        _currentValue = InventoryManager.Instance.currentBagValue;
        _maxValue = InventoryManager.Instance.maxBagValue;
        if (_currentValue != _maxValue)
        {
            Pickup();
        }
        else
        {
            Debug.Log("Bag is full... " + _currentValue);
        }
    }
}
