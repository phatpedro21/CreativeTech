using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixPathfindingPlease : MonoBehaviour {

	//activeNodes gives us a reference for all nodes, visited nodes contains nodes we've checked, remainingNodes are nodes still to b echecked
	List<GameObject> activeNodes = new List<GameObject>();
	List<GameObject> visitedNodes = new List<GameObject>();
	List<GameObject> remainingNodes = new List<GameObject>();
	List<KeyValuePair<GameObject, float>> values = new List<KeyValuePair<GameObject, float>>();
	GameObject startNode, endNode, currentNode;
	Stack<GameObject> path = new Stack<GameObject>();
	KeyValuePair<GameObject, float> bestPair;


	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Stack<GameObject> buildPath (GameObject _startNode, GameObject _endNode)
	{
		//CLEAR ALL
		activeNodes.Clear();
		remainingNodes.Clear();
		visitedNodes.Clear();
		path.Clear();
		values.Clear ();
		//Initialise
		startNode = _startNode;
		endNode = _endNode;
		List<GameObject> totalNodes = new List<GameObject>();
		totalNodes.AddRange(GameObject.FindGameObjectsWithTag ("Node"));
		for (int i = 0; i < totalNodes.Count; i++) 
		{
			if(totalNodes[i].GetComponent<Node>().enabled == true)
			{
				activeNodes.Add(totalNodes[i]);
			}
		}	
		visitedNodes.Add (startNode);
		remainingNodes.AddRange(activeNodes);
		remainingNodes.Remove (startNode);

		for (int i = 0; i < activeNodes.Count; i++)
		{
			if(i == activeNodes.IndexOf(startNode))
			{
				values.Add(new KeyValuePair<GameObject, float>(null, 0.0f));
			}
			else
			{
				values.Add(new KeyValuePair<GameObject, float>(null, -1.0f));
			}
		}


		//currentNode begins as startNode
		currentNode = startNode;

		//Check every node
		while (remainingNodes.Count > 0) 		
		{
			//if current node has connections;
			if(currentNode.GetComponent<Node>().connectedNodes.Count > 0)
			{
				//check the value for the path to each connected node. If it needs to be updated, update it
				//i for foreach loop
				int i = 0;
				foreach (GameObject node in currentNode.GetComponent<Node>().connectedNodes)
				{
					//if not yet been set, or value from this node is less than existing
					if(values[activeNodes.IndexOf(node)].Value == -1.0f ||
					   (values[activeNodes.IndexOf(currentNode)].Value + currentNode.GetComponent<Node>().pathCosts[i])  < values[activeNodes.IndexOf(node)].Value)
					{
						values[activeNodes.IndexOf(node)] = new KeyValuePair<GameObject,float>(currentNode,values[activeNodes.IndexOf(currentNode)].Value + currentNode.GetComponent<Node>().pathCosts[i]);
					}

					i++;
				}
			}

			//else find another node to look from
			else
			{
				//LEAVE FOR LATER
				Debug.Log("No route from here");

			}

			bestPair = new KeyValuePair<GameObject, float> (null, -1);
			
			for (int i = 0; i < values.Count; i++) 
			{
				if (!visitedNodes.Contains (activeNodes [i])) 
				{
					if (bestPair.Value == -1) 
					{
						bestPair = new KeyValuePair<GameObject, float> (activeNodes [i], values [i].Value);
					} 
					else if (values [i].Value < bestPair.Value && (values[i].Value != -1))
					{
						bestPair = new KeyValuePair<GameObject, float> (activeNodes[i], values [i].Value);
					}
				}
			}

			currentNode = bestPair.Key;
			visitedNodes.Add (currentNode);
			remainingNodes.Remove (currentNode);
		}

		path.Push(endNode);
		currentNode = endNode;
		while(currentNode != startNode)
		{
			currentNode = values[activeNodes.IndexOf(currentNode)].Key;
			path.Push(currentNode);
		}
		//path.Push(startNode);

	

		return path;
	}

}
