using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class BagItemController : MonoBehaviour
{
    public Item bagItem;

    public void RemoveItem()
    {
        BagManager.Instance.Remove(bagItem);
        
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        bagItem = newItem;
    }
}