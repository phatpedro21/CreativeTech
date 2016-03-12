using UnityEngine;
using System.Collections;


public class CollisionAvoidence : MonoBehaviour {

	public float turnSpeed;
	public float leftTurn, rightTurn;
	bool turning;

	//for direct collisions (how much to turn out of path)
	float turnAngle;

	// Use this for initialization
	void Start () {

		//turnAngle = Quaternion.AngleAxis (5, Vector3.up);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (transform.parent.GetComponent<CollisionAvoidenceTestMove> ())
		{
			if (transform.parent.GetComponent<CollisionAvoidenceTestMove> ().potentialCollisions.Count == 0) 
			{
				if (leftTurn > 0f) 
				{
					leftTurn -= turnSpeed * 3;
				}
				else 
				{
					leftTurn = 0f;
				}
				if (rightTurn > 0f) 
				{
					rightTurn -= turnSpeed * 3;
				} 
				else
				{
					rightTurn = 0f;
				}			
				this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().swerve (Quaternion.AngleAxis (leftTurn - rightTurn, new Vector3 (0f, 1f, 0f)));
			
			}
		}
		else if (this.transform.parent.GetComponent<BasicHuman>())
		{
			if (transform.parent.GetComponent<BasicHuman> ().potentialCollisions.Count == 0) 
			{
				if (leftTurn > 0f) 
				{
					leftTurn -= turnSpeed;
				}
				else 
				{
					leftTurn = 0f;
				}
				if (rightTurn > 0f) 
				{
					rightTurn -= turnSpeed;
				} 
				else
				{
					rightTurn = 0f;
				}			
				this.transform.parent.GetComponent<BasicHuman> ().setSwerve (Quaternion.AngleAxis (leftTurn - rightTurn, new Vector3 (0f, 1f, 0f)));
				
			}
		}

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.name != "Ground" && other.tag != "Node") 
		{
			if(this.transform.parent.GetComponent<CollisionAvoidenceTestMove>())
			{
				this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().potentialCollisions.Add (other.gameObject);
			}
			else if (this.transform.parent.GetComponent<BasicHuman>())
			{
				this.transform.parent.GetComponent<BasicHuman>().potentialCollisions.Add(other.gameObject);
			}
			turning = true;
		}

	}

	void OnTriggerStay(Collider other)
	{
		turning = true;
		if (other.name != "Ground" && other.tag != "CollisionBox" && other.tag != "Node") 
		{
			if (this.name.Contains ("Left"))
			{
				leftTurn += turnSpeed;
				//this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().swerve (Quaternion.AngleAxis (leftTurn, new Vector3 (0f, 1f, 0f)));
			}
			if (this.name.Contains ("Right")) 
			{
				rightTurn += turnSpeed;
				//this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().swerve (Quaternion.AngleAxis (-rightTurn, new Vector3 (0f, 1f, 0f)));
			}	
			if (transform.parent.GetComponent<CollisionAvoidenceTestMove> ())
			{
				this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().swerve (Quaternion.AngleAxis (leftTurn - rightTurn, new Vector3 (0f, 1f, 0f)));
			}
			else if (transform.parent.GetComponent<BasicHuman> ())
			{
				this.transform.parent.GetComponent<BasicHuman> ().setSwerve (Quaternion.AngleAxis (leftTurn - rightTurn, new Vector3 (0f, 1f, 0f)));	
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (transform.parent.GetComponent<CollisionAvoidenceTestMove> ())
		{
			this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().potentialCollisions.Remove (other.gameObject);
		}
		else if (transform.parent.GetComponent<BasicHuman> ())
		{
			this.transform.parent.GetComponent<BasicHuman> ().potentialCollisions.Remove (other.gameObject);
		}
		turning = false;
		if (other.name != "Ground") 
		{
			turning = false;
			if (this.name.Contains ("Left"))
			{
				//leftTurn = 0f;
				//this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().swerve (Quaternion.AngleAxis (leftTurn, new Vector3 (0f, 1f, 0f)));
			}
			if (this.name.Contains ("Right")) 
			{
				//rightTurn = 0f;
				//this.transform.parent.GetComponent<CollisionAvoidenceTestMove> ().swerve (Quaternion.AngleAxis (rightTurn, new Vector3 (0f, 1f, 0f)));
			}

		}
	}
}
