using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {
	
	int delay; //spawned = 0;
	float timeSinceSpawn;
	//public string humanFileName;
	public GameObject humanPrefab;
	GameObject spawnedHuman;
	public List<GameObject> otherDestinations = new List<GameObject>();
	public DestinationManager destinationManager;

	public bool _enabled = false;
	

	// Use this for initialization
	void Start () {
		timeSinceSpawn = 0;
		delay = Random.Range(3, 10);		

		//WILL GET VERY EXPENSIVE WITH MANY DESTINATIONS ? CHANGE IN FUTURE
		for (int i = 0; i < DestinationManager.destinations.Length; i++) 
		{
			if(DestinationManager.destinations[i].name != this.name)
			{
				otherDestinations.Add(DestinationManager.destinations[i]);
			}
		}
		
		
		
	}
	
	// Update is called once per frame
	void Update () {

		if (_enabled) 
		{
			timeSinceSpawn += Time.deltaTime;
		
			if (timeSinceSpawn > delay) {
				spawnedHuman = (GameObject)Instantiate (humanPrefab, transform.position, Quaternion.identity);
				if (otherDestinations.Count > 1) {
					spawnedHuman.GetComponent<BasicHuman> ().setDestination (otherDestinations [Random.Range (0, otherDestinations.Count - 1)]);
				} else {
					spawnedHuman.GetComponent<BasicHuman> ().setDestination (otherDestinations [0]);
				}
				delay = Random.Range (3, 10);
				timeSinceSpawn = 0;
			}
		}
		
	}
}

