using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Let the User have the control of the camera position
public class CameraController : MonoBehaviour {

	public float move_speed;
	public float distance_to_map;
	public float scrollSpeed;
	public float maxCamSize;
    public float minCamSize;
	private float boundary = 0.01f;
    private Vector3 MouseStart;
    private float dist;

    // Reference the map_Control to set the boundary of the camera
    private List<GameObject> tiles;
	private int map_size;

	// Use this for initialization
	void Start () {
		tiles = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>().map_ctr.map_tiles;
		map_size = GameObject.Find("WorldGenerator").GetComponent<WorldGenerator>().map_ctr.map_size;
		gameObject.GetComponent<Camera>().orthographicSize = 3.5f;
	}
	
	// Update is called once per frame
	void Update () {

		if(SceneManager.GetActiveScene().name != "TowerBase"){

            if (Input.GetMouseButtonDown(2))
            {
                MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                MouseStart = Camera.main.ScreenToWorldPoint(MouseStart);
                MouseStart.z = transform.position.z;

            }
            else if (Input.GetMouseButton(2))
            {
                var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                MouseMove = Camera.main.ScreenToWorldPoint(MouseMove);
                MouseMove.z = transform.position.z;
                transform.position = transform.position - (MouseMove - MouseStart);
            }

            if (transform.position.x <= tiles[tiles.Count - 1].transform.position.x + distance_to_map && (Input.GetKey("d"))){
				transform.Translate(new Vector3(move_speed * Time.deltaTime, 0, 0));
			}

			if (transform.position.x >= tiles[0].transform.position.x - distance_to_map && (Input.GetKey("a")))
            {
                transform.Translate(new Vector3(-move_speed * Time.deltaTime, 0, 0));
            }

			if (transform.position.y <= tiles[map_size - 1].transform.position.y + distance_to_map && (Input.GetKey("w")))
            {
				transform.Translate(new Vector3(0, move_speed * Time.deltaTime, 0));
            }

			if (transform.position.y >= tiles[map_size * (map_size - 1)].transform.position.y - distance_to_map && (Input.GetKey("s")))
            {
				transform.Translate(new Vector3(0, -move_speed * Time.deltaTime, 0));
            }

			if(gameObject.GetComponent<Camera>().orthographicSize <= maxCamSize && Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				gameObject.GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
			}

			if (gameObject.GetComponent<Camera>().orthographicSize >= minCamSize && Input.GetAxis("Mouse ScrollWheel") < 0)
            {
				gameObject.GetComponent<Camera>().orthographicSize += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSpeed;
            }
		}
	}
}
