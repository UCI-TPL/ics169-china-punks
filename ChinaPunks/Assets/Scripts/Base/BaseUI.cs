using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageEvent: UnityEvent<string>{}

public class BaseUI : MonoBehaviour {

	public MessageEvent errorEvent = new MessageEvent();
    
	private GameObject error_message;

	private void Awake()
	{
		errorEvent.AddListener(changeError);
		error_message = transform.GetChild(0).gameObject;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Change content of error message 
	public void changeError(string message){
		StartCoroutine(showMessage(message, 2));
	}

	IEnumerator showMessage(string message, int wait_seconds){
		error_message.SetActive(true);
        error_message.GetComponent<Text>().text = message;
		yield return new WaitForSeconds(wait_seconds);
		error_message.SetActive(false);
	}
}
