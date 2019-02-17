using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Let the User have the control of the camera position
public class CameraController : MonoBehaviour {

	public float speed;
	public float distance_to_map;
	private float boundary = 0.01f;

    // Reference the map_Control to set the boundary of the camera
	private List<GameObject> tiles;
	private int map_size;

	// Use this for initialization
	void Start () {
		tiles = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>().map_ctr.map_tiles;
		map_size = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>().map_ctr.map_size;
	}
	
	// Update is called once per frame
	void Update () {

		if(SceneManager.GetActiveScene().name != "TowerBase"){
			if(transform.position.x <= tiles[tiles.Count - 1].transform.position.x + distance_to_map && (Input.mousePosition.x > Screen.width - boundary || Input.GetKey("d"))){
				transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
			}

			if (transform.position.x >= tiles[0].transform.position.x - distance_to_map && (Input.mousePosition.x < 0 + boundary || Input.GetKey("a")))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }

			if (transform.position.y <= tiles[map_size - 1].transform.position.y + distance_to_map && (Input.mousePosition.y > Screen.height - boundary || Input.GetKey("w")))
            {
				transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }

			if (transform.position.y >= tiles[map_size * (map_size - 1)].transform.position.y - distance_to_map && (Input.mousePosition.y < 0 + boundary || Input.GetKey("s")))
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
