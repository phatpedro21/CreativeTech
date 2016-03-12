using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkBuild : MonoBehaviour {

	public List<GameObject> nodes;
	Stack<GameObject> path = new Stack<GameObject>();
	List<GameObject> visitedNodes = new List<GameObject>();
	List<GameObject> remainingNodes = new List<GameObject>();
	GameObject startNode, endNode;

	//dictionary of values to each node, and the 'previous' node from which to reach the current node (for backtracing to find path)
	List<KeyValuePair<GameObject, float>> values;
	int previousNodes;

	// Use this for initialization
	void Start () 
	{
		GameObject[] existingNodes = GameObject.FindGameObjectsWithTag ("Node");
		nodes = new List<GameObject> (existingNodes);
		values = new List<KeyValuePair<GameObject, float>> (existingNodes.Length);
		previousNodes = nodes.Count;
	}
	
	// Update is called once per frame
	void Update () 
	{		

		if (path.Count > 1)
		{
			Debug.DrawLine (path.Pop ().transform.position, path.Peek ().transform.position, Color.red, 3);
		}

		if ((GameObject.FindGameObjectsWithTag ("Node").Length + GameObject.FindGameObjectsWithTag ("StartNode").Length + GameObject.FindGameObjectsWithTag ("EndNode").Length) != previousNodes  ) 
		{
			updateAll ();

		}

		startNode = GameObject.FindGameObjectWithTag("StartNode");
		endNode = GameObject.FindGameObjectWithTag("EndNode");

	}




	public void buildPath()
	{
		updateAll ();
		startNode = GameObject.FindGameObjectWithTag("StartNode");
		endNode = GameObject.FindGameObjectWithTag("EndNode");
		//initialise visited nodes, unvisited nodes and values to each point
		int index = nodes.IndexOf(startNode);
		GameObject currentNode;
		visitedNodes.Add (startNode);
		remainingNodes = new List<GameObject>(nodes);
		remainingNodes.Remove (startNode);
		path.Clear ();

		for (int i = 0; i < nodes.Count; i++)
		{			
			values.Add(new KeyValuePair<GameObject, float>(null, -1));
		}
		values [index] =  new KeyValuePair<GameObject, float>(values[index].Key, 0.0f);

		currentNode = startNode;
		KeyValuePair<GameObject, float> bestPair = new KeyValuePair<GameObject, float>(null,-1);

		/*

		//Get value/cost to any nodes reachable from start node
		if(startNode.GetComponent<Node>().connectedNodes.Count > 0)
		{
			for(int i = 0; i < startNode.GetComponent<Node>().connectedNodes.Count;i++)
			{
				//If connected node has not already been visited (if it has already been visited do not return)
				GameObject nextNode = startNode.GetComponent<Node>().connectedNodes[i];
				if(!(visitedNodes.Contains(nextNode)))
				{
					//if cost of path is lower than exisitng path or path value has not been set, update the value for this path, and set which nodes we 
					//travveled from the get to this one
					if (startNode.GetComponent<Node>().pathCosts [i] < values [nodes.IndexOf (startNode.GetComponent<Node> ().connectedNodes [i])].Value
						|| values [nodes.IndexOf (startNode.GetComponent<Node> ().connectedNodes [i])].Value == -1f) 
					{
						values [nodes.IndexOf (startNode.GetComponent<Node> ().connectedNodes [i])] 
						= new KeyValuePair<GameObject, float>(startNode, startNode.GetComponent<Node>().pathCosts[i]);
					}

				}
			}
		}
		else
		{
			Debug.Log ("there is no route from this node");
		}



		for (int i = 0; i < values.Count; i++)
		{
			if(!visitedNodes.Contains(nodes[i]))
			{
				if (values[i].Value != -1) 
				{
					if (bestPair.Value == -1) 
					{
						bestPair = new KeyValuePair<GameObject, float> (nodes [i], values [i].Value);
					}
					if (values [i].Value < bestPair.Value) 
					{
						bestPair = new KeyValuePair<GameObject, float> (nodes [i], values [i].Value);
					}
				}
			}
		}	

		currentNode = bestPair.Key;
		visitedNodes.Add (currentNode);
		remainingNodes.Remove (currentNode);*/	

		//repeat until we arrive at destination
		while (remainingNodes.Count > 0)
		{
			//If there are other paths off of the current node, examine them - else break(for now) NEED TO MAKE IT CHECK ANOTHER NODE
			if (currentNode.GetComponent<Node> ().connectedNodes.Count > 0) 
			{
				for (int i = 0; i < currentNode.GetComponent<Node> ().connectedNodes.Count; i++) 
				{
					//If connected node has not already been visited (if it has already been visited do not return)
					GameObject nextNode = currentNode.GetComponent<Node> ().connectedNodes [i];
					if (!(visitedNodes.Contains (nextNode))) 
					{
						//if cost of path is lower than exisitng path or path value has not been set, update the value for this path, and set which nodes we 
						//travveled from the get to this one
						if ((currentNode.GetComponent<Node>().pathCosts [i] + values[nodes.IndexOf(currentNode)].Value < values [nodes.IndexOf (nextNode)].Value)
							|| values [nodes.IndexOf (nextNode)].Value == -1f) 
						{
							values [nodes.IndexOf (nextNode)] 
							= new KeyValuePair<GameObject, float>(currentNode, currentNode.GetComponent<Node>().pathCosts [i]  + values[nodes.IndexOf(currentNode)].Value);
						}
					}
				}
			} 
			else
			{
				Debug.Log ("there is no route from this node");
			}

			bestPair = new KeyValuePair<GameObject, float> (null, -1);

			for (int i = 0; i < values.Count; i++) 
			{
				if (!visitedNodes.Contains (nodes [i])) 
				{
					if (bestPair.Value == -1) 
					{
						bestPair = new KeyValuePair<GameObject, float> (nodes [i], values [i].Value);
					} 
					else if (values [i].Value < bestPair.Value && (values[i].Value != -1))
					{
						bestPair = new KeyValuePair<GameObject, float> (nodes [i], values [i].Value);
					}
				}
			}
				currentNode = bestPair.Key;
				visitedNodes.Add (currentNode);
				remainingNodes.Remove (currentNode);
		}


		//USING VALUES LIST WORK BACK TO FIND THE NODES ON THE PATH
		path = new Stack<GameObject>();
		currentNode = endNode;
		//while the current node (that we are looking back on) is not the start node
		while(values[nodes.IndexOf(currentNode)].Value != 0)
		{
			path.Push (currentNode);
			currentNode = values [nodes.IndexOf (currentNode)].Key;
		}
		path.Push (startNode);	

		values.Clear ();

		visitedNodes.Clear();
		remainingNodes.Clear ();

	}

	void updateAll()
	{
		nodes.Clear();
		nodes.AddRange(GameObject.FindGameObjectsWithTag ("Node"));
		if(GameObject.FindGameObjectWithTag("StartNode"))
		{
			nodes.Add(GameObject.FindGameObjectWithTag("StartNode"));
		}
		if(GameObject.FindGameObjectWithTag("EndNode"))
		{
			nodes.Add(GameObject.FindGameObjectWithTag("EndNode"));
		}
		
		values.Clear();
		values = new List<KeyValuePair<GameObject, float>> (nodes.Count);
		previousNodes = nodes.Count;
	}

}
