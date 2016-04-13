using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickToMoveShowcase : MonoBehaviour {

	//Selected agent
	public GameObject unit;
	//Level plane
	public GameObject ground;
	//
	public GameObject node;


	NetworkBuild _networkbuild;
	Stack<GameObject> _path;

	GameObject humanObj, destinationObj;
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
		//Finds mouse click position
		Ray mousePosToGround = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit mouseClick;
		//Debug.DrawRay (mousePosToGround.origin, mousePosToGround.direction * 100f, Color.red);
		
		Physics.Raycast (mousePosToGround, out mouseClick, 100.0f);
		
		if (Input.GetMouseButtonDown (0))
		{
			//if click on unit, assign this unit to current selected unit
			if (mouseClick.collider.tag == "Human")
			{		
				unit = mouseClick.collider.gameObject;
			}
			else if (mouseClick.collider.transform.parent.tag == "Human")
			{
				unit = mouseClick.collider.transform.parent.gameObject;
			}
		}

		if (Input.GetMouseButtonDown (1))
		{
			//if we already have a unit selected, create a destination for this unit and create path
			if(unit != null)
			{

				Vector3 instantiatePoint;
				instantiatePoint = mouseClick.point;
				instantiatePoint.y = 1;
				//create new node at destination
				destinationObj = (GameObject)Instantiate(node, instantiatePoint, Quaternion.identity);
				destinationObj.transform.name = "DESTINATION";

				//create new node at human position
				humanObj = (GameObject)Instantiate(node, unit.transform.position, Quaternion.identity);
				humanObj.transform.name = "START";

				//make sure these nodes have connections
				destinationObj.GetComponent<Node>().addNodes();
				humanObj.GetComponent<Node>().addNodes();
			}
		}

		//if a agent is selected and has a destination set for it, build path
		if(destinationObj != null && destinationObj.GetComponent<Node>().connectedNodes.Count > 0 && humanObj != null  && humanObj.GetComponent<Node>().connectedNodes.Count > 0)
		{
			unit.GetComponent<AgentShowcase>().destPos = destinationObj.transform.position;
			FindObjectOfType<PathfindingShowcase>().callBuildPath(humanObj, destinationObj, unit);
			Destroy(destinationObj);
			Destroy(humanObj);
		}			
		
	}
}
