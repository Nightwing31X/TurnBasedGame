using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] bool _buttonClicked = false;
    [SerializeField] bool _playerINV = false;


    public void OnDrop(PointerEventData eventData)
    {
        if (_playerINV)
        {
            Debug.Log("I am now inside the player's INV - No longer Bag");
        }
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;
        }
        else
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

            GameObject current = transform.GetChild(0).gameObject;
            DraggableItem currentDraggable = current.GetComponent<DraggableItem>();

            currentDraggable.transform.SetParent(draggableItem.parentAfterDrag);
            draggableItem.parentAfterDrag = transform;
        }
    }


    public void ClickedButton()
    {
        Debug.Log("Something???");
        _buttonClicked = !_buttonClicked;

        transform.Find("Item").GetComponent<DraggableItem>().updateTextData();
        
    }
}
