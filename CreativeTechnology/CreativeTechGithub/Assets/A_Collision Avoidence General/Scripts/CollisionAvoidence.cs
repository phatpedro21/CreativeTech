using UnityEngine;
using System.Collections;


public class CollisionAvoidence : MonoBehaviour {

	public float turnSpeed;
	public float leftTurn, rightTurn;
	public bool turningLeft, turningRight;

	//for direct collisions (how much to turn out of path)
	float turnAngle;

	// Use this for initialization
	void Start () {

        turningLeft = false;
        turningRight = false;

		//turnAngle = Quaternion.AngleAxis (5, Vector3.up);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
       /* if (turningRight && this.name.Contains("Left"))
        {
            Debug.Log("Right");
            //Debug.Break();
            rightTurn += turnSpeed;      
            if(Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f)) != transform.parent.GetComponent<CollisionManager>().angle)
            {
                transform.parent.GetComponent<CollisionManager>().updateAngle(Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f)));
            }      
           // this.transform.parent.GetComponent<BasicHuman>().setSwerve((Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f))));
            Debug.DrawRay(this.transform.position, (Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f)) * this.transform.forward), Color.red);    
        }
        else if (turningLeft && this.name.Contains("Right"))
        {
            Debug.Log("Left");
            leftTurn += turnSpeed;
            if (Quaternion.AngleAxis(leftTurn, new Vector3(0f, 1f, 0f)) != transform.parent.GetComponent<CollisionManager>().angle)
            {
                transform.parent.GetComponent<CollisionManager>().updateAngle(Quaternion.AngleAxis(-leftTurn, new Vector3(0f, 1f, 0f)));
            }
            // this.transform.parent.GetComponent<BasicHuman>().setSwerve((Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f))));
            Debug.DrawRay(this.transform.position, (Quaternion.AngleAxis(-leftTurn, new Vector3(0f, 1f, 0f)) * this.transform.forward), Color.red);
        }

        if (!turningRight && rightTurn > 0f)
        {        
           rightTurn -= turnSpeed;
        }
        else
        {
           rightTurn = 0f;          
        }
        if (Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f)) != transform.parent.GetComponent<CollisionManager>().angle)
        {
             transform.parent.GetComponent<CollisionManager>().updateAngle(Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f)));
        }               
        
        if (!turningLeft && leftTurn > 0f)
        {
           leftTurn -= turnSpeed;
        }
        else
        { 
           leftTurn = 0f;          
        }
        if (Quaternion.AngleAxis(rightTurn, new Vector3(0f, 1f, 0f)) != transform.parent.GetComponent<CollisionManager>().angle)
        {
            transform.parent.GetComponent<CollisionManager>().updateAngle(Quaternion.AngleAxis(-leftTurn, new Vector3(0f, 1f, 0f)));
        }
        if(!turningLeft && !turningRight && leftTurn == 0f && rightTurn == 0f)
        {
            transform.parent.transform.parent.GetComponent<BasicHuman>().swerve = new Quaternion(0f, 0f, 0f, 0f);
        }*/
    
     }


	void OnTriggerEnter(Collider other)
	{
        if (other.tag.Contains("Human") || other.tag.Contains("Obstacle"))
        {
            if (this.name.Contains("Left"))
            {
                transform.parent.GetComponent<CollisionManager>().turningRight = true;
            }
            if (this.name.Contains("Right"))
            {
                transform.parent.GetComponent<CollisionManager>().turningLeft = true;
            }
        }


    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag.Contains("Human") || other.tag.Contains("Obstacle"))
        {
            if (this.name.Contains("Left") && !transform.parent.GetComponent<CollisionManager>().turningRight)
            {
                transform.parent.GetComponent<CollisionManager>().turningRight = true;
            }
            if (this.name.Contains("Right") && !transform.parent.GetComponent<CollisionManager>().turningLeft)
            {
                transform.parent.GetComponent<CollisionManager>().turningLeft = true;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag.Contains("Human") || other.tag.Contains("Obstacle"))
        {
            if (this.name.Contains("Left"))
            {
                transform.parent.GetComponent<CollisionManager>().turningRight = false;
            }
            if (this.name.Contains("Right"))
            {
                transform.parent.GetComponent<CollisionManager>().turningLeft = false;
            }
        }

    }

}



/*
if (transform.parent.GetComponent<CollisionAvoidenceTestMove> ())
		{
			if (transform.parent.GetComponent<CollisionAvoidenceTestMove> ().potentialCollisions.Count == 0) 
			{

                if (leftTurn > 0f) 
				{
                   leftTurn *= 0.9f;
				}
				else
                {
                   //leftTurn = 0f;
				}
				if (rightTurn > 0f) 
				{
                   rightTurn *= 0.9f; 
                } 
				else
                {
                   //rightTurn = 0f;
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
                    Debug.Log("DECREASInGTURN");
                    leftTurn -= turnSpeed;
				}
				else 
				{
                    Debug.Log("UNTURN");
					leftTurn = 0f;
				}
				if (rightTurn > 0f) 
				{
                    Debug.Log("DECREASInGTURN");
                    rightTurn -= turnSpeed;
				} 
				else
                {
                    Debug.Log("UNTURN");
                    rightTurn = 0f;
				}			
				this.transform.parent.GetComponent<BasicHuman> ().setSwerve (Quaternion.AngleAxis (leftTurn + rightTurn, new Vector3 (0f, 1f, 0f)));
				
			}
		}

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

*/
