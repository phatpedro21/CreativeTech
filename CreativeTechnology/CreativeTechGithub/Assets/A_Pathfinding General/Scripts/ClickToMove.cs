using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickToMove : MonoBehaviour {

	public GameObject unit;
	public GameObject ground;
	public GameObject node;
	public GameObject unitToMove;
	GameObject humanNode;
	GameObject destinationNode;
	NetworkBuild _networkbuild;
	Stack<GameObject> _path;


	GameObject start, end;

	GameObject currentMoveLocation;

	// Use this for initialization
	void Start () 
	{
		ground = GameObject.Find("Ground");
		_networkbuild = FindObjectOfType<NetworkBuild> ();

	}
	
	// Update is called once per frame
	void Update () 
	{

		Ray mousePosToGround = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit mouseClick;
		Debug.DrawRay (mousePosToGround.origin, mousePosToGround.direction, Color.red);

		Physics.Raycast (mousePosToGround, out mouseClick, 100.0f);

		if (Input.GetMouseButtonDown (0))
		{
			if (mouseClick.collider.tag == "Human") {
				Debug.Log ("YOU SELECTED A HUMAN");
				unit = mouseClick.collider.gameObject;
			}
			else if (mouseClick.collider.transform.parent.tag == "Human")
			{
				unit = mouseClick.collider.transform.parent.gameObject;
			}
		}
		if (Input.GetMouseButtonDown (1))
		{
			if(unit != null)
			{
				Debug.Log("YOU SET A DESTINATION");
				destinationNode = (GameObject)Instantiate(node, mouseClick.point, Quaternion.identity);
				destinationNode.transform.name = "DESTINATION";
				humanNode = (GameObject)Instantiate(node, unit.transform.position, Quaternion.identity);
				humanNode.transform.name = "START";
				destinationNode.GetComponent<Node>().addNodes();
				humanNode.GetComponent<Node>().addNodes();
			}
		}

		if(destinationNode != null && destinationNode.GetComponent<Node>().connectedNodes.Count > 0 && humanNode != null  && humanNode.GetComponent<Node>().connectedNodes.Count > 0)
		{
			unit.GetComponent<BasicHuman>().destPos = destinationNode.transform.position;
			//unit.GetComponent<BasicHuman>().path = new Stack<GameObject>(FindObjectOfType<FixPathfindingPlease>().buildPath(humanNode, destinationNode));
			unit.GetComponent<BasicHuman>().pathList = new List<GameObject>(FindObjectOfType<FixPathfindingPlease>().buildPath(humanNode, destinationNode));
			Destroy(destinationNode);
			destinationNode = null;
			Destroy(humanNode);
			humanNode = null;
		}


		if (_path != null && _path.Count > 0  )
		{
			while(_path.Count > 1)
			{
				GameObject prev = _path.Pop();
				Debug.DrawLine(prev.transform.position, _path.Peek().transform.position, Color.red, 10.0f);
			}
		}


		//unit.GetComponent<>

	 
	}
}
