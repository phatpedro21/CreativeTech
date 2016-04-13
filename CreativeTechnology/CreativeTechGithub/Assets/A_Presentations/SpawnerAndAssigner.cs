using UnityEngine;
using System.Collections;

public class SpawnerAndAssigner : MonoBehaviour {

	public GameObject agent;

	// Use this for initialization
	void Start () {		

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void spawn()
	{
			//Spawn new agent;
			GameObject instantiatedAgent;
			GameObject destination;

			FindObjectOfType<DestinationList> ().canSpawn = false;

			destination = FindObjectOfType<DestinationList>().Destinations[(int)Random.Range(0, FindObjectOfType<DestinationList>().Destinations.Count)];
			while (destination == this.gameObject) 
			{
				destination = FindObjectOfType<DestinationList>().Destinations[(int)Random.Range(0, FindObjectOfType<DestinationList>().Destinations.Count)];
			}		

			if (agent != null) 
			{
				
				instantiatedAgent = (GameObject)Instantiate (agent, transform.position, Quaternion.identity);
				instantiatedAgent.GetComponent<AgentShowcase> ().destPos = destination.transform.position;
			FindObjectOfType<PathfindingShowcase> ().callBuildPath (this.gameObject, destination, instantiatedAgent); 
				
				Debug.Log (destination.name);
			}

	}
	

}
