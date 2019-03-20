using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    // Reference to the item UI
    public GameObject description_UI;

    public string description = "";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        description_UI.SetActive(true);
        description_UI.transform.GetChild(0).gameObject.GetComponent<Text>().text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description_UI.SetActive(false);
    }
}
