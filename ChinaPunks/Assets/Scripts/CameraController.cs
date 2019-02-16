using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Let the User have the control of the camera position
public class CameraController : MonoBehaviour {

	public float speed;
	private float boundary = 0.01f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(SceneManager.GetActiveScene().name != "TowerBase"){
			if(transform.position.x <= 10 && (Input.mousePosition.x > Screen.width - boundary || Input.GetKey("d"))){
				transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
			}

			if (transform.position.x >= -10 && (Input.mousePosition.x < 0 + boundary || Input.GetKey("a")))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }

			if (transform.position.y <= 10 && (Input.mousePosition.y > Screen.height - boundary || Input.GetKey("w")))
            {
				transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }

			if (transform.position.y >= -10 && (Input.mousePosition.y < 0 + boundary || Input.GetKey("s")))
            {
				transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
            }

			if(gameObject.GetComponent<Camera>().orthographicSize <=10 && Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				gameObject.GetComponent<Camera>().orthographicSize++;
			}

			if (gameObject.GetComponent<Camera>().orthographicSize >= 2 && Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                gameObject.GetComponent<Camera>().orthographicSize--;
            }
		}
	}
}
