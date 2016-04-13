using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingShowcase : MonoBehaviour {


	//For showing search space
	public bool showCheckedNodes;
	public Material visitedNodeColor;
	int coroutineNo = 1;

	//For making pathfinding add another cost(distance to end)
	public bool distanceCheck;

	public bool yieldYes;

	
	public bool callBuildPath(GameObject _startNode, GameObject _endNode, GameObject _unit)
	{
		StartCoroutine(buildPath(_startNode, _endNode, _unit));
		return true;
	}
	
	IEnumerator buildPath (GameObject _startNode, GameObject _endNode, GameObject _unit)
	{
		int thisCoroutineNo = coroutineNo;
		coroutineNo++;
		List<Node> activeNodes = new List<Node>();
		List<Node> visitedNodes = new List<Node>();
		List<Node> remainingNodes = new List<Node>();
		List<KeyValuePair<Node, float>> values = new List<KeyValuePair<Node, float>>();
		Stack<Vector3> path = new Stack<Vector3>();
		KeyValuePair<Node, float> bestPair;
		int iter = new int();


		GameObject startObj, endObj;
		Node startNode, endNode, currentNode;	

		//DEBUG TIMER
		float startTime, endTime, diffTime;		
		
		startTime = Time.realtimeSinceStartup;

		//CLEAR ALL
		activeNodes.Clear();
		remainingNodes.Clear();
		visitedNodes.Clear();
		path.Clear();
		values.Clear ();


		//Initialise
		startObj = (GameObject)Instantiate(_startNode, _startNode.transform.position, Quaternion.identity);
		endObj = (GameObject)Instantiate (_endNode, _endNode.transform.position, Quaternion.identity);
		endNode = endObj.GetComponent<Node> ();
		startNode = startObj.GetComponent<Node>();
		endNode.addNodes ();
		startNode.addNodes ();
	
		yield return null;

		List<GameObject> totalNodes = new List<GameObject>();
		totalNodes.AddRange(GameObject.FindGameObjectsWithTag("Node"));

		for (int i = 0; i < totalNodes.Count; i++) 
		{
			if(totalNodes[i].GetComponent<Node>().enabled == true)
			{
				activeNodes.Add(totalNodes[i].GetComponent<Node>());
			}
		}

		startNode.gameObject.tag = "NullNode";
		endNode.gameObject.tag = "NullNode";

		yield return null;


		visitedNodes.Add (startNode);
		remainingNodes.AddRange(activeNodes);
		remainingNodes.Remove (startNode);
		
		for (int i = 0; i < activeNodes.Count; i++)
		{
			if(i == activeNodes.IndexOf(startNode))
			{
				values.Add(new KeyValuePair<Node, float>(null, 0.0f));
			}
			else
			{
				values.Add(new KeyValuePair<Node, float>(null, -1.0f));
			}
		}
		
		//currentNode begins as startNode
		currentNode = startNode;
		
		//Check every node
		while (currentNode != endNode) 	         	
		{
			//if current node has connections;
			if(currentNode.GetComponent<Node>().connectedNodes.Count > 0)
			{
				//check the value for the path to each connected node. If it needs to be updated, update it
				//i for foreach loop
				iter = 0;
				foreach (GameObject node in currentNode.GetComponent<Node>().connectedNodes)
				{	
					
						if (endObj != null) {
							if (!distanceCheck) {
								//if not yet been set, or value from this node is less than existing
								if (node != endNode) {
									if (values [activeNodes.IndexOf (node.GetComponent<Node> ())].Value == -1.0f ||
									   (values [activeNodes.IndexOf (currentNode)].Value + currentNode.pathCosts [iter]) < values [activeNodes.IndexOf (node.GetComponent<Node> ())].Value) {
										values [activeNodes.IndexOf (node.GetComponent<Node> ())] = new KeyValuePair<Node, float> (currentNode, values [activeNodes.IndexOf (currentNode)].Value + currentNode.pathCosts [iter]);
									}
								}
							}
							if (distanceCheck) 
						{
							
								float distVal = 0;
								//distVal += (Vector3.Distance (node.GetComponent<Node> ().position, endNode.position) - Vector3.Distance (currentNode.position, endNode.position));
								distVal += Vector3.Distance (node.GetComponent<Node> ().position, endNode.position) - Vector3.Distance (_unit.transform.position, endNode.position);
								//distVal += Vector3.Distance (currentNode.position, _unit.transform.position);

								if (values [activeNodes.IndexOf (node.GetComponent<Node> ())].Value == -1.0f ||
								   (values [activeNodes.IndexOf (currentNode)].Value + currentNode.pathCosts [iter] + distVal) < values [activeNodes.IndexOf (node.GetComponent<Node> ())].Value && !visitedNodes.Contains (node.GetComponent<Node> ())) 
							{
									values [activeNodes.IndexOf (node.GetComponent<Node> ())] = new KeyValuePair<Node, float> (currentNode, (values [activeNodes.IndexOf (currentNode)].Value + currentNode.pathCosts [iter] + distVal));
								}
							}
						}
					}

					iter++;


			}
			//else find another node to look from
			else
			{
				//LEAVE FOR LATER
				//Debug.Log("No route from here");
				break;				
			}
			
			bestPair = new KeyValuePair<Node, float> (null, -1);
			for (int i = 0; i < values.Count; i++) 
			{
				if (!visitedNodes.Contains (activeNodes [i])) 
				{
					if (bestPair.Value == -1) 
					{
						if(values[i].Value != -1)
						{
							bestPair = new KeyValuePair<Node, float> (activeNodes[i], values [i].Value);
						}
					}
					else if(values[i].Value != -1 && (values[i].Value < bestPair.Value))
					{
						bestPair = new KeyValuePair<Node, float> (activeNodes[i], values [i].Value);
					}

				}
			}

			if (showCheckedNodes) {
				Debug.DrawLine(currentNode.position, bestPair.Key.position, Color.red, 10f);
				currentNode.gameObject.GetComponent<Renderer> ().material = visitedNodeColor;
				yield return null;

			}
			else if (yieldYes) 
			{
				yield return null;
			}

			currentNode = bestPair.Key;
			visitedNodes.Add (currentNode);

			remainingNodes.Remove (currentNode);			
		
		}				
		path.Push(endNode.position);
		currentNode = endNode;
		while(currentNode != startNode)
		{
			currentNode = values[activeNodes.IndexOf(currentNode)].Key;           
			path.Push(currentNode.position); 
			if (path.Count > 30)
			{
				yield break;
			}
		}

		//path.Push(startNode);
		endTime = Time.realtimeSinceStartup;
		Debug.Log(endTime - startTime); 

		yield return null;
		if (_unit.GetComponent<AgentShowcase> ()) 
		{
			_unit.GetComponent<AgentShowcase> ().callFindTarget ();
			_unit.GetComponent<AgentShowcase> ().pathList = new List<Vector3> (path);
		}
		if (_unit.GetComponent<SpawnerAndAssigner> ()) 
		{
			//_unit.GetComponent<SpawnerAndAssigner> ().potentialPaths.Add(new List<Vector3> (path));
		}
		Destroy (startObj);

		Destroy (endObj);
		yield break;
		
	}

}
