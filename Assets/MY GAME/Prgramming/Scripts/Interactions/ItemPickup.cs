using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item bagItem;

    private int _maxBagValue;
    private int _currentBagValue;
    private bool _bagFull;


    void Awake()
    {
        if (bagItem == null)
        {
            Debug.LogWarning("Have not put a Scriptable Object...");
        }
    }
    void Pickup()
    {
        _currentBagValue = BagManager.Instance.currentBagItemValue;
        _maxBagValue = BagManager.Instance.maxBagNumber;
        _bagFull = BagManager.Instance.isBagFull;

        if (_currentBagValue == _maxBagValue)
        {
            Debug.Log("Bag is full...");
            _bagFull = true;
        }
        else
        {
            BagManager.Instance.Add(bagItem);
            Destroy(gameObject);
        }


    }

    public void Interact()
    {
        Pickup();
    }
}
