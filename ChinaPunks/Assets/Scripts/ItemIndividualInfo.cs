using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIndividualInfo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Item item; //init an empty item
    public int ItemCurrentSlotIndex;
    InventoryHUD inventoryInfo;

    // Use this for initialization
    void Start()
    {
        inventoryInfo = GameObject.Find("ItemDatabase").GetComponent<InventoryHUD>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.SetParent(transform.parent.parent);
            this.transform.position = eventData.position;
            this.transform.localScale = new Vector2(1.0f, 1.0f);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            this.transform.position = eventData.position;
            this.transform.localScale = new Vector2(1.0f, 1.0f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.localScale = new Vector2(0.75f, 0.75f);
        this.transform.SetParent(inventoryInfo.slotInfoList[ItemCurrentSlotIndex].transform);
        this.transform.position = this.transform.parent.position; // reset position
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
