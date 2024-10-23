using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public Item item;

    void Pickup()
    {
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
    }

    public void Interact()
    {
        Pickup();
    }
}
