using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class ItemDataBase : MonoBehaviour {

    private JsonData itemData;
    private List<Item> database = new List<Item>();

    // Use this for initialization
    void Start () {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/items.json", Encoding.GetEncoding("GB2312"))); // read json file
        ContructItemDatabase();
        //Debug.Log(FetchItemByID(1).Title + FetchItemByID(1).Desp);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ContructItemDatabase()
    {
        // init item database
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new Item((int)itemData[i]["id"],
                                       itemData[i]["title"].ToString(),
                                  (int)itemData[i]["value"],
                                       itemData[i]["description"].ToString(),
                                       itemData[i]["slug"].ToString()));
        }
    }

    public Item FetchItemByID(int _id)
    {
        // find item by id
        for (int i = 0; i < database.Count; i++)
        {
            if (_id == database[i].ID)
            {
                return database[i];
            }
        }
        return null;
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public string Desp { get; set; }
    public Sprite sprite { get; set; }

    public Item(int _id, string _title, int _value, string _desp, string _slug)
    {
        this.ID = _id;
        this.Title = _title;
        this.Value = _value;
        this.Desp = _desp;
        this.sprite = Resources.Load<Sprite>("Items/" + _slug);
    }

    public Item()
    {
        this.ID = -1; // id = -1 means item is Empty
    }
}
