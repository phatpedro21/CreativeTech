using UnityEngine;
using System.Collections;

public class RayInteceptTest : MonoBehaviour {

	public GameObject pA1, pA2, pB1, pB2;

	// Use this for initialization
	void Start () {

		checkIntersect ();
	
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawLine (pA1.transform.position, pA2.transform.position);
		Debug.DrawLine (pB1.transform.position, pB2.transform.position);
	
	}

	void checkIntersect()
	{
		//Intersect check code here
	    //http://www.wyrmtale.com/blog/2013/115/2d-line-intersection-in-c
		 
		//pA1.transform.position
		// Get A,B,C of first line - points : ps1 to pe1
		float A1 = pA1.transform.position.z - pA2.transform.position.z;
		float B1 = pA2.transform.position.x - pA1.transform.position.x;
		float C1 = A1 * pA2.transform.position.x + B1 * pA2.transform.position.z;

		// Get A,B,C of second line - points : ps2 to pe2
		float A2 = pB1.transform.position.z - pB2.transform.position.z;;
		float B2 = pB2.transform.position.x - pB1.transform.position.x;
		float C2 = A2 * pB2.transform.position.x + B2 * pB2.transform.position.z;
	
		/*// Get A,B,C of second line - points : ps2 to pe2
		float A2 = pe2.y - ps2.y;
		float B2 = ps2.x - pe2.x;
		float C2 = A2 * ps2.x + B2 * ps2.y;*/
	
		// Get delta and check if the lines are parallel
		float delta = A1 * B2 - A2 * B1;
		if (delta == 0) 
		{
			//lines are parralell
		}
		else 
		{
			// now return the Vector2 intersection point
			Vector3 intersect = new Vector3 ((B2 * C1 - B1 * C2) / delta, pA1.transform.position.y, (A1 * C2 - A2 * C1) / delta);

			Instantiate (pA1, intersect, Quaternion.identity);
		}
	}

}
