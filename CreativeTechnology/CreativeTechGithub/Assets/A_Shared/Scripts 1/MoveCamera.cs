using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {

	public float moveSpeed, zoomSpeed;

	// Use this for initialization
	void Start () {

		moveSpeed = 0.7f;
		zoomSpeed = 0.5f;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.UpArrow))
		{
			this.GetComponent<Camera>().transform.position -= new Vector3(moveSpeed,0.0f,0.0f);
		}
		if(Input.GetKey(KeyCode.DownArrow ))
		{
			this.GetComponent<Camera>().transform.position += new Vector3(moveSpeed,0.0f,0.0f);
		}
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			this.GetComponent<Camera>().transform.position -= new Vector3(0.0f,0.0f,moveSpeed);
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			this.GetComponent<Camera>().transform.position += new Vector3(0.0f,0.0f,moveSpeed);
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0.0f)
		{
			this.GetComponent<Camera>().orthographicSize -= zoomSpeed;
		}
		
		if (Input.GetAxis ("Mouse ScrollWheel") < 0.0f)
		{
			this.GetComponent<Camera>().orthographicSize += zoomSpeed;
		}
	}
}
