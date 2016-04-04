using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixPathfindingPlease : MonoBehaviour {

	//activeNodes gives us a reference for all nodes, visited nodes contains nodes we've checked, remainingNodes are nodes still to b echecked
	List<Node> activeNodes = new List<Node>();
	List<Node> visitedNodes = new List<Node>();
	List<Node> remainingNodes = new List<Node>();
	List<KeyValuePair<Node, float>> values = new List<KeyValuePair<Node, float>>();
	//GameObject startNode, endNode, currentNode;
	Stack<Vector3> path = new Stack<Vector3>();
	KeyValuePair<Node, float> bestPair;

    Node startNode, endNode, currentNode;


    //DEBUG TIMER
    float startTime, endTime, diffTime;


	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Stack<Vector3> callBuildPath(GameObject _startNode, GameObject _endNode, GameObject unit)
    {
        StartCoroutine(buildPath(_startNode, _endNode, unit));
        return path;
    }

	IEnumerator  buildPath (GameObject _startNode, GameObject _endNode, GameObject unit)
	{
        startTime = Time.realtimeSinceStartup;
        //CLEAR ALL
        activeNodes.Clear();
		remainingNodes.Clear();
		visitedNodes.Clear();
		path.Clear();
		values.Clear ();
		//Initialise
		startNode = _startNode.GetComponent<Node>();
        endNode = _endNode.GetComponent<Node>();       
		List<GameObject> totalNodes = new List<GameObject>();
		totalNodes.AddRange(GameObject.FindGameObjectsWithTag ("Node"));
		for (int i = 0; i < totalNodes.Count; i++) 
		{
            if(totalNodes[i].GetComponent<Node>().enabled == true)
			{
				activeNodes.Add(totalNodes[i].GetComponent<Node>());
			}
		}	
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
				int i = 0;
				foreach (GameObject node in currentNode.GetComponent<Node>().connectedNodes)
				{

					//if not yet been set, or value from this node is less than existing
					if(values[activeNodes.IndexOf(node.GetComponent<Node>())].Value == -1.0f ||
					   (values[activeNodes.IndexOf(currentNode)].Value + currentNode.GetComponent<Node>().pathCosts[i])  < values[activeNodes.IndexOf(node.GetComponent<Node>())].Value)
					{
                        values[activeNodes.IndexOf(node.GetComponent<Node>())] = new KeyValuePair<Node, float>(currentNode.GetComponent<Node>(),
                            values[activeNodes.IndexOf(currentNode)].Value + currentNode.GetComponent<Node>().pathCosts[i] + Vector3.Distance(currentNode.transform.position, endNode.transform.position));
						                      
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

			bestPair = new KeyValuePair<Node, float> (null, -1);
			
			for (int i = 0; i < values.Count; i++) 
			{
				if (!visitedNodes.Contains (activeNodes [i])) 
				{
					if (bestPair.Value == -1) 
					{
						bestPair = new KeyValuePair<Node, float> (activeNodes[i], values [i].Value);
					} 
					else if (values [i].Value < bestPair.Value && (values[i].Value != -1))
					{
						bestPair = new KeyValuePair<Node, float> (activeNodes[i], values [i].Value);
					}
				}
			}

			currentNode = bestPair.Key;
			visitedNodes.Add (currentNode);
			remainingNodes.Remove (currentNode);
            yield return null;
		}

		path.Push(endNode.position);
		currentNode = endNode;
		while(currentNode != startNode)
		{
            currentNode = values[activeNodes.IndexOf(currentNode)].Key;           
            path.Push(currentNode.position);
			yield return null;
		}
        //path.Push(startNode);

		FindObjectOfType<ClickToMove> ().destroyNodes();      
        endTime = Time.realtimeSinceStartup;
        Debug.Log(endTime - startTime);
		unit.GetComponent<BasicHuman>().pathList = new List<Vector3>(path);
		Debug.Break ();

		
	}

}
