using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentShowcase : MonoBehaviour {
	
	//PRE VO OBJECT WORK
	public GameObject destination;
	GameObject originalDestination;
	Rigidbody _thisRigidbody;
	string humanFileName;
	public Node thisObjNode; 

	public bool smoothPaths;

	Vector3 accel = Vector3.zero;
	Vector3 swerveVector = Vector3.zero;
	public Quaternion swerve;

	public Vector3 testVec =  Vector3.zero;


	public float speed;
	
	//Human Needs
	public float needA, needB, needC;
	
	public List<GameObject> potentialCollisions = new List<GameObject> ();
	
	public List<Vector3> pathList;
	
	public int humanNo;	
	
	public Vector3 destPos;
	Vector3? currentTargetPos;	
	
	//For the pathfinding smoothing, this will hold the next node if it can be seen from current pos;
	RaycastHit _nodeSearch;
	//index of 'seeable' node
	int checkTarget = 0;	

	GameObject target;
	
	//POST VO OBJECT WORK
	public Vector3 desiredVely;
	
	// Use this for initialization
	void Start ()
	{
		currentTargetPos = null;
		_thisRigidbody = GetComponent<Rigidbody> ();	

		//if the city showcase, assign random speed for variation
		if (FindObjectOfType<DestinationList> ()) 
		{
			speed = Random.Range (1, 2.5f);
		}
		
		needA = Random.Range (0, 20);
		needB = Random.Range (1, 20);
		needC = Random.Range (1, 20);
	}
	
	// Update is called once per frame
	void Update () 
	{

		//For showcase testing
		if (Input.GetKeyDown (KeyCode.G)) 
		{
			if (speed == 0)
			{
				speed = 1;
			} 
			else 
			{
				speed = 0;
			}
		}
        //Looks in direction of movement
        transform.LookAt (this.transform.position + swerve * desiredVely);

		if (pathList != null)
		{
			for (int i = 0; i < pathList.Count - 1; i++)
			{
				Debug.DrawLine (pathList [i], pathList [i + 1], Color.blue, 50f);
			}
		} 
		/*if (pathList.Count == 0)
		{			
			GameObject destination;
			List<GameObject> nodes = new List<GameObject> ();
			nodes.AddRange(GameObject.FindGameObjectsWithTag ("Node"));
			destination = nodes [Random.Range (0, nodes.Count)];
			FindObjectOfType<PathfindingShowcase> ().callBuildPath(this.gameObject, destination, this.gameObject);
		}*/

		if (pathList.Count != 0)
		{				
			if (currentTargetPos != null && Vector3.Distance (transform.position, (Vector3)currentTargetPos) < 0.2)
			{
				StartCoroutine (findTarget ());
			}
		}
          
		if (Vector3.Distance(transform.position, destPos) < 0.5f)
		{
			desiredVely = Vector3.zero;
			currentTargetPos = null;
		} 

        if(currentTargetPos != null)
        {			
 			{
				desiredVely = Vector3.Normalize ((Vector3)currentTargetPos - transform.position);
			}
        }

		if(FindObjectOfType<DestinationList>() && Vector3.Distance(transform.position, destPos) < 0.5)
		{
			Destroy(this.gameObject);
		}

        //Debug.Log(pathList.Count);		
		
    }
	
	void FixedUpdate()
	{
		this.transform.GetComponent<Rigidbody> ().velocity = swerve * desiredVely * speed;      
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Attractor") 
		{
			originalDestination = destination;
			destination =  other.gameObject;
			Invoke("ResetDestination", 0.5f);
			
		}
		if (other.tag == "Detractor") 
		{
			desiredVely += (other.transform.position - transform.position);
		}
	}
	
	public void setSwerve(Quaternion angle)
	{
		if (angle != Quaternion.identity)
		{
			swerve = angle;
		}

		//Debug.DrawRay(this.transform.position - new Vector3(0f, 0f, 2f), swerve * this.transform.forward, Color.white);
	}
	
	void ResetDestination ()
	{
		destination = originalDestination;
	}
	
	public void setDestination (GameObject l_destination)
	{
		destination = l_destination;
	}

	public void callFindTarget()
	{
		StartCoroutine (findTarget ());
	}
	
IEnumerator findTarget()
{
	while (checkTarget < pathList.Count - 1)
		{
			//int layerMask = (1 << 8) | (1 << 9);
			//pathList[checkTarget].layer = 8;
			Physics.Raycast(pathList[0], (pathList[checkTarget] - pathList[0]), out _nodeSearch);
			//Debug.DrawRay (pathList[0], (pathList[checkTarget] - pathList[0]), Color.white, 3f);
			if (_nodeSearch.collider != null && _nodeSearch.collider.tag == "Obstacle" )
			{
				
				if (checkTarget == 0 || checkTarget == 1) 
				{		
					currentTargetPos = pathList [0];
                    pathList.RemoveAt(0);
                }

				currentTargetPos = pathList [checkTarget - 1];
				if (smoothPaths) {
					Debug.DrawLine (pathList [0], (Vector3)currentTargetPos, Color.red, 50f);
				}
				pathList.RemoveRange (0, checkTarget - 1);

				checkTarget = 0;				
					
				yield break;
			}         
			else
			{
				
				if (_nodeSearch.collider)
				{
					//Debug.Log(_nodeSearch.collider.name);
				}
			}            
			checkTarget++;  
			//yield return null;            
		}

		currentTargetPos = destPos;
		destPos.y = transform.position.y;
		if (smoothPaths) {
			Debug.DrawLine (pathList [0], (Vector3)currentTargetPos, Color.red, 50f);
		}
		checkTarget = 0;		
		pathList.Clear();
		yield break; 	
		
}
	

}