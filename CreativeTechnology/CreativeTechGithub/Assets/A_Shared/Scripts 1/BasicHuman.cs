using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BasicHuman : MonoBehaviour {

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

	public List<GameObject> potentialCollisions = new List<GameObject> ();

	public List<Vector3> pathList;

	public int humanNo;


    public Vector3 destPos;
    Vector3? currentTargetPos;
 

    //For the pathfinding smoothing, this will hold the next node if it can be seen from current pos;
    RaycastHit _nodeSearch;
    //index of 'seeable' node
    int checkTarget = 1;



    public Stack<GameObject> path;
	GameObject target;

	//POST VO OBJECT WORK
	public Vector3 desiredVely;

	// Use this for initialization
	void Start ()
	{
        currentTargetPos = null;
		_thisRigidbody = GetComponent<Rigidbody> ();	
	}
	
	// Update is called once per frame
	void Update () 
	{

       
        /*if (currentTargetPos != null && Vector3.Distance(transform.position, (Vector3)currentTargetPos) >= 1) 
        {
            Debug.Log("MOVING");
            desiredVely = Vector3.Normalize((Vector3)currentTargetPos - transform.position);
        }
        else if (pathList.Count > 0)
        {
            Debug.Log("FINDING TARGET");
            StartCoroutine(findTarget());
            // Debug.Log("LOOKING");
            
        }*/

		for(int i = 0; i < pathList.Count - 1; i++)
		{
			Debug.DrawLine(pathList[i], pathList[i+1], Color.blue, 3f);
		}

        transform.LookAt (this.transform.position + swerve * desiredVely);
		/*if (pathList.Count > 0 && pathList[0] == null) 
		{
			pathList.RemoveAt(0);	
			for(int i = 0; i < pathList.Count - 2; i ++)
			{
				Debug.DrawLine(pathList[i].transform.position, pathList[i+1].transform.position, Color.blue, 5.0f);
			}
			target = pathList[0];
		}

		if (target != null)
		{
			float dist = Vector3.Distance (this.transform.position, target.transform.position);
		}
		//if at current target, get next node
		if (target != null && Vector3.Distance (this.transform.position, target.transform.position) < 0.5f)
		{					
			if(pathList.Count > 2)
			{
				pathList.RemoveAt(0);	
				target = pathList[0];
			}
			else if (pathList.Count == 1)
			{
				target = pathList[0];
			}
			else
			{
				target = null;
				desiredVely = swerve * Vector3.Normalize (destPos - this.transform.position) * speed; 
			}

		}
		if (Vector3.Distance (this.transform.position, destPos) < 0.5f)
		{
			desiredVely = Vector3.zero;
		}

		if (target != null) 
		{
			//POST VO OBJECT WORK
			desiredVely = swerve * Vector3.Normalize (target.transform.position - this.transform.position) * speed; 
			//Debug.Log (Vector3.Distance (this.transform.position, target.transform.position));
		}*/

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
            Physics.Raycast(transform.position, (pathList[checkTarget] - transform.position), out _nodeSearch);
            if (_nodeSearch.collider.transform.position != pathList[checkTarget])
            {
                Debug.Log("Hit obstacle" + _nodeSearch.collider.name);                
                currentTargetPos = pathList[checkTarget - 1];
                pathList.RemoveRange(1, checkTarget - 1);
                checkTarget = 1;
                Debug.DrawLine(transform.position, (Vector3)currentTargetPos, Color.red, 4);
                yield break;
            }
            if (_nodeSearch.collider.gameObject.layer == 8)
            {
                
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
                   
            
        }
        currentTargetPos = destPos;
        Debug.DrawLine(transform.position, (Vector3)currentTargetPos, Color.red, 4);
        pathList.Clear();
        yield break;    

           /* if (!(_nodeSearch.collider.name.Contains("Node")))
            {
                Debug.Log("MOVING TO " + pathList[checkTarget].name);
                currentTargetPos = pathList[checkTarget].transform.position;
                pathList.RemoveRange(1, checkTarget);
                checkTarget = 1;
                Debug.DrawLine(transform.position, (Vector3)currentTargetPos, Color.red, 4);                
                yield break;
            }
            pathList[checkTarget].layer = 0;
            yield return null;

            checkTarget++;
        }
       
        currentTargetPos = destPos;
        pathList.Clear();
        yield break;
        */
    }

	void OnDrawGizmos ()
	{
		//Gizmos.color = Color.red;
		//Gizmos.DrawWireSphere (transform.position, 10f);

		//Gizmos.DrawRay (transform.position, accel);
	}

	/*
	 * Removed Stuff
	 * 
	 * First Attempt at avoidance... Works just about, couldnt figure out how when to recalculate new direction
	 * 
	 * Vector3 avoidOtherHuman = Vector3.zero;
		RaycastHit _hit;

		Debug.DrawRay(transform.position, destDirection * 20, Color.black); 
		//if(Physics.SphereCast(transform.position,0.5f ,destDirection, out _hit, 20))

		if (_thisRigidbody.SweepTest(destDirection, out _hit, 20))
		{
			if (_hit.transform.tag == "Human") 
			{	

				if(swerveVector == Vector3.zero)
				{
					float swerveAmount = Random.Range (-1.0f, 1.0f);
					swerveAmount = (swerveAmount == 0) ? 0.5f : swerveAmount;
					swerveAmount *= 30;
					
					swerveVector = Quaternion.AngleAxis (swerveAmount, Vector3.up) * transform.forward;		
				}
				if(_thisRigidbody.SweepTest (swerveVector, out _hit, 20))
				{
					if (_hit.transform.tag == "Human") 
					{
						float swerveAmount = Random.Range (-1.0f, 1.0f);
						swerveAmount = (swerveAmount == 0) ? 0.5f : swerveAmount;
						swerveAmount *= 30;

						swerveVector = Quaternion.AngleAxis (swerveAmount, Vector3.up) * transform.forward;			
					}	
				}


//original pathfinding stuff. 

/*if (path != null && target == null) {
			target = path.Pop ();
		} 

		if (target != null) 
		{
			//POST VO OBJECT WORK
			desiredVely = Vector3.Normalize (target.transform.position - this.transform.position) * speed; 
			Debug.Log(Vector3.Distance(this.transform.position, target.transform.position));
			if(Vector3.Distance(this.transform.position, target.transform.position) < 0.5f)		
			{

				//NEED TO FIND WAY TO MAKE IT TO LAST DEST THEN STOP. WILL CURRENTLY CARRY ON :S:S:S:S

				if(path.Count > 1)
				{
					target = path.Pop();
				}
				else
				{
					path = null;
					target = path.Pop();
				}
			}
}*/
	
	
	
	
	//path = FindObjectOfType<FixPathfindingPlease>().buildPath(humanNode, destinationNode);
	
	/*if (path != null&& target == null) 
		{
			path.Pop();
			if(path.Peek() != null)
			{
				target = path.Pop();
			}
			else
			{
				desiredVely = Vector3.Normalize (destPos - this.transform.position) * speed; 
			}
		} */


	 /* 
	 * 
	 * 
	 *
	 */



	                         
}
