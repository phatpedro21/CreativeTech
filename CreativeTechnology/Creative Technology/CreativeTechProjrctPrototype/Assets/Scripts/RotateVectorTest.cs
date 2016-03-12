using UnityEngine;
using System.Collections;

public class RotateVectorTest : MonoBehaviour {

	[Range(0.0f, 360.0f)]
	public int x;
	[Range(0.0f, 360.0f)]
	public int y;
	[Range(0.0f, 360.0f)]
	public int z;

	Vector3 vector;


	// Use this for initialization
	void Start () {

		vector = new Vector3 (100, 0, 0);	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Quaternion rotation = Random.rotation;
		vector = Quaternion.AngleAxis (x, Vector3.up) * vector;


		//Debug.DrawRay (Vector3.zero, vector, Color.red);
	}
}
