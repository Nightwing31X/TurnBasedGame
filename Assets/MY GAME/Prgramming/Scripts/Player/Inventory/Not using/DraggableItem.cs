using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Interactions;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform parentAfterDrag;

    public Item bagItem;
    public Text title;
    public Text description;
    public RawImage image;

    public Toggle enableRemoveToggle;


    void Start()
    {
        bagItem = GetComponent<InventoryItemController>().item;

        // image.texture = item.artwork;
        title = GameObject.Find("IconTitleText").GetComponent<Text>();
        description = GameObject.Find("IconDescriptionText").GetComponent<Text>();
        image = transform.Find("ItemIcon").GetComponent<RawImage>();
        enableRemoveToggle = GameObject.Find("EnableRemove").GetComponent<Toggle>();
        // title.text = "Name of the Item";
        // description.text = "Description about the item...";

        // image.texture = item.artwork;
        // title.text = item.name;

    }

    public void updateTextData()
    {
        title.text = bagItem.name;
        description.text = bagItem.description;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        if (enableRemoveToggle.isOn)
        {
            enableRemoveToggle.isOn = false;
        }

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        title.text = bagItem.name;
        description.text = bagItem.description;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
