using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentShowcase : MonoBehaviour {
	
	//PRE VO OBJECT WORK
	public GameObject destination;
	GameObject originalDestination;
	Rigidbody _thisRigidbody;
	string humanFileName;

	Vector3 accel = Vector3.zero;
	Vector3 swerveVector = Vector3.zero;
	public Quaternion swerve;

	public Vector3 testVec =  Vector3.zero;


	public int speed = 2;
	
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
		
		needA = Random.Range (0, 20);
		needB = Random.Range (1, 20);
		needC = Random.Range (1, 20);
	}
	
	// Update is called once per frame
	void Update () 
	{
        //Looks in direction of movement
        transform.LookAt (this.transform.position + swerve * desiredVely);

		if (pathList != null) 
		{
			for(int i = 0; i < pathList.Count - 1; i++)
			{
				Debug.DrawLine(pathList[i], pathList[i+1], Color.blue, 10f);
			}
		}

        if (pathList.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("FINDING TARGET");
                StartCoroutine(findTarget());

            }			            
		}

        if(currentTargetPos != null)
        {
            desiredVely = Vector3.Normalize((Vector3)currentTargetPos - transform.position);
        }

        Debug.Log(pathList.Count);		
		
    }
	
	void FixedUpdate()
	{
		this.transform.GetComponent<Rigidbody> ().velocity = swerve * desiredVely;      
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
		//Debug.DrawRay(this.transform.position - new Vector3(0f,0f, 2f), angle * this.transform.forward, Color.white);
		Debug.DrawRay(this.transform.position - new Vector3(0f, 0f, 2f), swerve * this.transform.forward, Color.white);
	}
	
	void ResetDestination ()
	{
		destination = originalDestination;
	}
	
	public void setDestination (GameObject l_destination)
	{
		destination = l_destination;
	}
	
	IEnumerator findTarget()
	{
		while (checkTarget < pathList.Count - 1)
			{
				//int layerMask = (1 << 8) | (1 << 9);
				//pathList[checkTarget].layer = 8;
				Physics.Raycast(pathList[0], (pathList[checkTarget] - pathList[0]), out _nodeSearch);
				Debug.DrawRay (pathList[0], (pathList[checkTarget] - pathList[0]), Color.white, 3f);
				if (_nodeSearch.collider != null && _nodeSearch.collider.tag == "Obstacle" )
				{
					Debug.Log("Hit obstacle" + _nodeSearch.collider.name);
					if (checkTarget == 0 || checkTarget == 1) 
					{
                        currentTargetPos = pathList[0];
                        pathList.RemoveAt(0);
                    }
					currentTargetPos = pathList[checkTarget - 1];
                Debug.DrawLine(pathList[0], (Vector3)currentTargetPos, Color.red, 10f);
                pathList.RemoveRange(0, checkTarget - 1);
					checkTarget = 0;
					
					yield break;
				}         
				else
				{
					Debug.Log("WHAT?");
					if (_nodeSearch.collider)
					{
						Debug.Log(_nodeSearch.collider.name);
					}
				}            
				checkTarget++;  
				yield return null;            
			}
			currentTargetPos = destPos;
			Debug.DrawLine(pathList[0], (Vector3)currentTargetPos, Color.red, 10f);
			checkTarget = 0;
			pathList.Clear();
			yield break; 	
		
	}
	

}