using UnityEngine;
using System.Collections;

public class SpawnerAndAssigner : MonoBehaviour {

	public GameObject agent;
	float timer, randomTime;

	// Use this for initialization
	void Start () {
		
		randomTime = Random.Range (8, 10);
		timer = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {

		if ((Time.timeSinceLevelLoad - timer) > randomTime)
		{
			//Spawn new agent;
			GameObject instantiatedAgent;

			GameObject destination;

			destination = FindObjectOfType<DestinationList>().Destinations[(int)Random.Range(0, FindObjectOfType<DestinationList>().Destinations.Count)];
			while (destination == this.gameObject) 
			{
				destination = FindObjectOfType<DestinationList>().Destinations[(int)Random.Range(0, FindObjectOfType<DestinationList>().Destinations.Count)];
			}

			if (agent != null) 
			{
				randomTime = Random.Range (8, 10);
				timer = Time.timeSinceLevelLoad;
				instantiatedAgent = (GameObject)Instantiate (agent, transform.position, Quaternion.identity);
				FindObjectOfType<PathfindingShowcase> ().callBuildPath (this.gameObject, destination, instantiatedAgent); 
				Debug.Log (destination.name);
			}

		}
	
	}
}
