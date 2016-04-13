using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestinationList : MonoBehaviour {


	float timer, randomTime;
	GameObject spawner;


	public List<GameObject> Destinations;
	public bool canSpawn;


	// Use this for initialization
	void Start () {	
		randomTime = Random.Range (2, 10);
		timer = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {	


		if ((Time.timeSinceLevelLoad - timer) > randomTime)
		{
			randomTime = Random.Range (4, 6);
			timer = Time.timeSinceLevelLoad;
			spawner = Destinations[Random.Range(0, Destinations.Count)];
			while(!spawner.GetComponent<SpawnerAndAssigner>())
			{
				spawner = Destinations[Random.Range(0, Destinations.Count)];
			}

			spawner.GetComponent<SpawnerAndAssigner> ().spawn ();
		}

	}



}
