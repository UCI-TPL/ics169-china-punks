using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHUD : MonoBehaviour {

    public GameObject Slot_prefab;
    public GameObject Item_prefab;

    public List<Item> itemDataList = new List<Item>();
    public List<GameObject> slotInfoList = new List<GameObject>();

    GameObject slotPanel;
    public int itemsNumber = 15;

    ItemDataBase itemdatabase;

    // Use this for initialization
    void Start () {
        itemdatabase = GetComponent<ItemDataBase>();

        slotPanel = GameObject.Find("Slot_Panel");

        Init_database();
    }

    public void Init_database()
    {
        for (int i = 0; i < itemsNumber; i++)
        {
            slotInfoList.Add(Instantiate(Slot_prefab));
            slotInfoList[i].transform.SetParent(slotPanel.transform);  //set auto layout 
            slotInfoList[i].GetComponent<SlotIndividualInfo>().slotIndividualID = i;
            itemDataList.Add(new Item()); // init item data list
        }

        AddItemByIDFromDataBase(0);
        AddItemByIDFromDataBase(0);
        AddItemByIDFromDataBase(1);
    }
	
	public void AddItemByIDFromDataBase(int _id)
    {
        Item ItemToAdd = itemdatabase.FetchItemByID(_id);
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if(itemDataList[i].ID == -1)
            {
                itemDataList[i] = ItemToAdd;
                GameObject itemObj = Instantiate(Item_prefab);
                itemObj.transform.SetParent(slotInfoList[i].transform); //auto layout 
                RectTransform trans = itemObj.GetComponent<RectTransform>();
                trans.localPosition = Vector3.zero;
                itemObj.name = ItemToAdd.Title;
                itemObj.GetComponent<Image>().sprite = ItemToAdd.sprite; // change sprite
                itemObj.GetComponent<ItemIndividualInfo>().item = ItemToAdd; // Set Item Individual Info
                itemObj.GetComponent<ItemIndividualInfo>().ItemCurrentSlotIndex = i; // Set Item Individual Info
                break; // find empty item for itemtoadd
            }
        }
    }
}
