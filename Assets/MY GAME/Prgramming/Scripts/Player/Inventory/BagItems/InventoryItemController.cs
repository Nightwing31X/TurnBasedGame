using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public Item item;
    // private Item _bagItem;
    private Text _title;
    private Text _description;

    void Awake()
    {
        _title = GameObject.Find("IconTitleText").GetComponent<Text>();
        _description = GameObject.Find("IconDescriptionText").GetComponent<Text>();
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);

        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void ItemDetails()
    {
        _title.text = item.name;
        _description.text = item.description;
    }
}