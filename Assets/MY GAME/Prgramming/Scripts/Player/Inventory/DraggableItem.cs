using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// public class DraggableItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler 
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// public class DraggableItem : MonoBehaviour
{
    [HideInInspector] public Transform parentAfterDrag;

    public Item item;
    public Text title;
    public Text description;
    public RawImage image;

    void Start()
    {
        item = GetComponent<InventoryItemController>().item;

        // image.texture = item.artwork;
        title = GameObject.Find("IconTitleText").GetComponent<Text>();
        description = GameObject.Find("IconDescriptionText").GetComponent<Text>();
        image = transform.Find("ItemIcon").GetComponent<RawImage>();
        // title.text = "Name of the Item";
        // description.text = "Description about the item...";

        // image.texture = item.artwork;
        // title.text = item.name;
    }

    public void updateTextData()
    {
        title.text = item.name;
        description.text = item.description;
    }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     title.text = item.name;
    //     description.text = item.description;
    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;

        title.text = item.name;
        description.text = item.description;
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

    // public void DefaultText()
    // {
    //     title.text = "Name of the Item";
    //     description.text = "Description about the item...";
    // }
}
