﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {


	//******************************************//
	//******************************************//
	// Is clicknode?
	// Destroy()
	// UpdateConnections()
	// 
	//
	//
	//
	//

	//Holds connectedNodes and the distance between them
	List<GameObject> knownNodes = new List<GameObject> ();
	public List<GameObject> connectedNodes = new List<GameObject>();
	public List<float> pathCosts = new List<float>();
    public Vector3 position;
	static float s_checkRadius;
	public float checkRadius;

	//Holds amount of nodes 'before' a new one is added, for checking if it needs to update connectedNodes list
	int nodes;

	// is this a input based location (as opposed to node in level)
	bool isClickedLocation = false;


	// Use this for initialization
	void Start () 
	{
        position = transform.position;
		addNodes ();
		if (this.tag == "EndNode") 
		{
			FindObjectOfType<NetworkBuild>().buildPath();			
			Destroy(this.gameObject);
		}
		checkRadius = s_checkRadius;
	}

	void OnEnable()
	{


	}
	
	// Update is called once per frame
	void Update () 
	{
		
		addNodes ();

		for (int i = 0; i < knownNodes.Count; i++) 
		{
			if (knownNodes [i].GetComponent<Node> ().enabled == false) {
				connectedNodes.RemoveAt (i);
				//pathCosts.RemoveAt(i);
				nodes = connectedNodes.Count;
			}

		}

		UpdatePathCosts ();	
	}

	void UpdatePathCosts()
	{
		//clear path cost data
		pathCosts.Clear ();
		foreach (GameObject node in connectedNodes)
		{
			pathCosts.Add (Vector3.Distance (this.transform.position, node.transform.position));		
		}

		Destroy (this.GetComponent<BoxCollider>());
	}

	public void addNodes()
	{
		int prevKnowNodesCount = knownNodes.Count;
		knownNodes.Clear ();	

		Collider[] adjacentNodes = Physics.OverlapSphere (this.transform.position, checkRadius);
		foreach (Collider node in adjacentNodes) 
		{
			if((node.tag == "Node" || node.tag == "ANode" || node.tag == "BNode" || node.tag == "CNode" || node.tag == "NullNode") && node.gameObject != this.gameObject && node.GetComponent<Node>().enabled == true)
			{
				knownNodes.Add(node.gameObject);
			}
		}

		if (knownNodes.Count > prevKnowNodesCount) 
		{

			connectedNodes = knownNodes;
		
			//At start, if this node has multiple connections, add distance value to costs list
			if (connectedNodes.Count > 1) 
			{
				foreach (GameObject node in connectedNodes) 
				{
					pathCosts.Add (Vector3.Distance (this.transform.position, node.transform.position));
				}	
			}

			//else if only one connection, set less expensively
			else 
			{
				pathCosts.Add (Vector3.Distance (this.transform.position, connectedNodes [0].transform.position));
			}
		}

		for (int i = 0; i < connectedNodes.Count; i++)
		{
			Debug.DrawLine(this.transform.position, connectedNodes[i].transform.position);
		}

		//sets the number of nodes originally connected to this node;
		nodes = knownNodes.Count;
	
	}


	void OnTriggerEnter(Collider other)
	{

		if (other.tag == "Obstacle") 
		{
			if (this.tag == "StartNode" || this.tag == "EndNode") 
			{
				this.tag = "Node";
			}		
	
			this.enabled = false;
		}
	}


	void OnTriggerExit(Collider other)
	{
		if (this.tag == "StartNode" || this.tag == "EndNode")
		{
			this.tag = "Node";
		}

		//Says Hello!		
		this.enabled = true;
	}


	public void setCheckRadius(float radius)
	{
		if (s_checkRadius == 0) 
		{
			s_checkRadius = radius;
		}
	}


}

