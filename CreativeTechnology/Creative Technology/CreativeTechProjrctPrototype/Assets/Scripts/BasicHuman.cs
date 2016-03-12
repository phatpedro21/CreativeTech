using UnityEngine;
using System.Collections;


public class BasicHuman : MonoBehaviour {

	//PRE VO OBJECT WORK
	public GameObject destination;
	GameObject originalDestination;
	Rigidbody _thisRigidbody;
	string humanFileName;
	Vector3 accel = Vector3.zero;
	Vector3 swerveVector = Vector3.zero;
	public Vector3 testVec =  Vector3.zero;
	public int speed = 2;
	public GameObject testCube;


	//POST VO OBJECT WORK
	public Vector3 desiredVely;
	DestinationManager _currentDestManager;



	/*
	 * variables (MKI)...
	 * float Speed?
	 * gameobject ? overlap physics? Vision?
	 * 
	*/

	// Use this for initialization
	void Start ()
	{
		testCube = GameObject.Find ("TestCube");
		_currentDestManager = FindObjectOfType<DestinationManager> ();

		_thisRigidbody = GetComponent<Rigidbody> ();

	
	}
	
	// Update is called once per frame
	void Update () 
	{

		//POST VO OBJECT WORK
		desiredVely = Vector3.Normalize (destination.transform.position - this.transform.position) * speed; 

		//PRE VO OBJECT WORK
		/*
		vely = change in position
		acceleration = change in vely
		*/

		/*//move to destination
		Vector3 toDest = Vector3.zero;
		Vector3 destDirection = Vector3.zero;
		if (destination != null) 
		{
			toDest = destination.transform.position;
			destDirection = toDest - transform.position;
		}

		Collider[] nearbyAgents = Physics.OverlapSphere (this.transform.position, 5.0f);
		if(nearbyAgents.Length > 0)
		{
			for(int i = 0; i < nearbyAgents.Length;i++)
			{
				if(nearbyAgents[i].tag == "Human")
				{
					Vector3 collisionPoint = Vector3.zero;
					//_thisRigidbody.velocity.sqrMagnitude
					collisionPoint =   checkIntersect(this.transform.position,this.transform.position + transform.forward, 
						               nearbyAgents[i].transform.position, nearbyAgents[i].transform.position + nearbyAgents[i].transform.forward);

					if(!(collisionPoint.y > 6000))
					{

						toDest = nearbyAgents[i].transform.position + new Vector3(0.5f,0,0);
					}
				}

			}
		}


		transform.LookAt (new Vector3(toDest.x, 0.0f, toDest.z));

		//velocity = change in time
		//accleration = change in velocity

		Debug.DrawRay (transform.position, transform.forward * 10, Color.blue);

		_thisRigidbody.velocity = transform.forward * speed;*/


		if(Vector3.Distance(transform.position, destination.transform.position) < 0.5)
		{
			Destroy(this);			
		}

	}

	void FixedUpdate()
	{
		this.transform.GetComponent<Rigidbody> ().velocity = desiredVely;
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

	void ResetDestination ()
	{
		destination = originalDestination;
	}

	public void setDestination (GameObject l_destination)
	{
		destination = l_destination;
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

	 * 
	 * 
	 * 
	 */

	                         
}
