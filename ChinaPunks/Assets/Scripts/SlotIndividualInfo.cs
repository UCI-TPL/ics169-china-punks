using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotIndividualInfo : MonoBehaviour, IDropHandler {

    public int slotIndividualID;
    InventoryHUD inventoryInfo;

    // Use this for initialization
    void Start () {
        inventoryInfo = GameObject.Find("ItemDatabase").GetComponent<InventoryHUD>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemIndividualInfo DroppedItemInfo = eventData.pointerDrag.GetComponent<ItemIndividualInfo>();
        if(inventoryInfo.itemDataList[slotIndividualID].ID == -1)
        {
            inventoryInfo.itemDataList[DroppedItemInfo.ItemCurrentSlotIndex] = new Item(); // clean last slot info of dropped item
            DroppedItemInfo.ItemCurrentSlotIndex = slotIndividualID; //set new slot index for this item
            inventoryInfo.itemDataList[slotIndividualID] = DroppedItemInfo.item; // update item info in inventory data list 
        }
        else if (DroppedItemInfo.ItemCurrentSlotIndex != slotIndividualID)
        {
            Transform Current_item = this.transform.GetChild(0);
            Current_item.GetComponent<ItemIndividualInfo>().ItemCurrentSlotIndex = DroppedItemInfo.ItemCurrentSlotIndex;
            Current_item.transform.SetParent(inventoryInfo.slotInfoList[DroppedItemInfo.ItemCurrentSlotIndex].transform);
            Current_item.transform.position = Current_item.transform.parent.position; // reset position

            inventoryInfo.itemDataList[DroppedItemInfo.ItemCurrentSlotIndex] = Current_item.GetComponent<ItemIndividualInfo>().item; // clean last slot info of dropped item
            DroppedItemInfo.ItemCurrentSlotIndex = slotIndividualID; //set new slot index for this item
            inventoryInfo.itemDataList[slotIndividualID] = DroppedItemInfo.item; // update item info in inventory data list 
        }
    }
}
