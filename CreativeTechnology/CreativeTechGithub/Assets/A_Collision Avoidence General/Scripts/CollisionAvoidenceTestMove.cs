using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionAvoidenceTestMove : MonoBehaviour {

	Vector3 target;
	public float speed;
	Vector3 velocity;
	public Quaternion swerveAngle;
	public List<GameObject> potentialCollisions = new List<GameObject> ();
	float closestObstacle;

	public bool facing;

	// Use this for initialization
	void Start () {
		if (facing) {
			target = this.transform.position + new Vector3 (0f, 0f, -100f);
		}
		else 
		{
			target = this.transform.position + new Vector3 (0f, 0f, 100f);
		}
	
	}
	
	// Update is called once per frame
	void Update () {

		/*Ray _ray = Camera.current.ScreenPointToRay (Input.mousePosition);
		RaycastHit _hit;
		Physics.Raycast(_ray, out _hit);

		if (_hit.collider.name == "Ground")
		{
			target = _hit.point;
		}*/
		closestObstacle = 0f;
		for (int i = 0; i < potentialCollisions.Count; i++)
		{
			if (i == 0)
			{
				closestObstacle = Vector3.Distance (this.transform.position, potentialCollisions [i].transform.position);
			}
			else if (Vector3.Distance (this.transform.position, potentialCollisions [i].transform.position) < closestObstacle)
			{
				closestObstacle = Vector3.Distance (this.transform.position, potentialCollisions [i].transform.position);
			}
		}

		this.transform.LookAt (this.transform.position + velocity);
		if (closestObstacle > 0 && closestObstacle < speed)
		{
			velocity = swerveAngle * (Vector3.Normalize (target - this.transform.position) * closestObstacle);
		} 
		else 
		{
			velocity = swerveAngle * (Vector3.Normalize (target - this.transform.position) * speed);
		}
	}


	void FixedUpdate()
	{
		this.GetComponent<Rigidbody> ().velocity = velocity;
	}

	void OnTriggerStay()
	{
	}


	public void swerve(Quaternion angle)
	{
		swerveAngle = angle;
	}
}
