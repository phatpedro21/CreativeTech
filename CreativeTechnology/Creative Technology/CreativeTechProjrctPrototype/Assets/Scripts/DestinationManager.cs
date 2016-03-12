using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestinationManager : MonoBehaviour {

	public static DestinationManager _destinationManager = null;
	//List<GameObject> destinations = new List<GameObject> ();
	public static GameObject[] destinations;

	void Awake ()
	{
		if (_destinationManager == null) {
			_destinationManager = this;
			DontDestroyOnLoad (this);
		}
		else if (_destinationManager != this)
		{
			Destroy(this);
		}


		destinations = GameObject.FindGameObjectsWithTag("Destination");
		
		for(int i = 0; i < destinations.Length; i++)
		{
			Debug.Log(destinations[i]);
		}
	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
