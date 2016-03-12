using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VelocityObjectTest : MonoBehaviour {

	public GameObject objectA, objectB;
	Vector3 VOLeftSide, VORightSide;

	public List<GameObject> otherAgentList = new List<GameObject> ();
	List<Vector3> VOPointsList =  new List<Vector3> ();
	List<Vector3> CrossOverPointsList =  new List<Vector3> ();
	Mesh mesh;

	public GameObject VelyObject;
	Vector3 objAVely = new Vector3(1, 0, 1);
	Vector3 objBVely = new Vector3(-1, 0, -1);
	Vector3 closestPointA;
	Vector3 closestPointB;

	//DEBUG CUBE MADNESS PLEASE SEND HELP
	List<GameObject> instantatedCubes = new List<GameObject> ();

	//float deltaTime = 0.0f;

	// Use this for initialization
	void Start () {

		VOLeftSide = Vector3.zero;
		VORightSide = Vector3.zero;
		otherAgentList.Add (objectB);
		//otherAgentList.Add (objectC);
		VelyObject.AddComponent<MeshFilter>();
		VelyObject.AddComponent<MeshRenderer>();
		mesh = VelyObject.GetComponent<MeshFilter>().mesh;

	}
	
	// Update is called once per frame
	void Update () 
	{

		//Clear lists ready for next tick
		VOPointsList.Clear ();
		CrossOverPointsList.Clear ();
		//Sets a default velocity
		//velyA = new Vector3 (velyAX, velyAY, velyAZ);

		//bool edgeFound = false;
		//Sets raycasting direction for first check
		Vector3 rayDirection = objectB.transform.position - objectA.transform.position;
		RaycastHit _hit;
		VOLeftSide = Vector3.zero;
		VORightSide = Vector3.zero;
		VOPointsList.Clear ();

		//Runs VO checks against each nearby agent
		foreach (GameObject agent in otherAgentList) 
		{
			//Updates which object the ray is looking at
			rayDirection = agent.transform.position - objectA.transform.position;

			//Ray cast sweeps to the right to find right side of ObjectB collision zone
			while (VORightSide == Vector3.zero)
			{
				Physics.Raycast (objectA.transform.position, rayDirection, out _hit);
				rayDirection = Quaternion.AngleAxis (1, Vector3.up) * rayDirection;
				if (_hit.collider != agent.GetComponent<Collider> ()) {
					VORightSide = objectA.transform.position + rayDirection;
				}
			}

			//Resets ray direction to start from center of objectB again
			rayDirection = agent.transform.position - objectA.transform.position;
			//Ray cast sweeps to the left to find left side of ObjectB collision zone
			while (VOLeftSide == Vector3.zero) {
				Physics.Raycast (objectA.transform.position, rayDirection, out _hit);			
				rayDirection = Quaternion.AngleAxis (-1, Vector3.up) * rayDirection;
				if (_hit.collider != agent.GetComponent<Collider> ()) {
					VOLeftSide = objectA.transform.position + rayDirection;	
				}
			}

			//Builds VO and moves it to accomodate for other agents movement
			Vector3 VOLeftPoint = VOLeftSide + objBVely;
			VOPointsList.Add (VOLeftPoint);

			Vector3 VORightPoint = VORightSide + objBVely;
			VOPointsList.Add (VORightPoint);

			Vector3 VOBasePoint = objectA.transform.position + objBVely;
			VOPointsList.Add (VOBasePoint);

			VOLeftSide = Vector3.zero;
			VORightSide = Vector3.zero;

			mesh.Clear();

			mesh.vertices = new Vector3[] {VOPointsList[0], VOPointsList[1], VOPointsList[2]};
			//mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
			mesh.triangles = new int[] {0, 1, 2};


			Mesh VOTri = VelyObject.GetComponent<MeshFilter>().mesh;
			VOTri.Clear();

			VOTri.vertices = new Vector3[] {VOPointsList[0], VOPointsList[1], VOPointsList[2]};
			VOTri.triangles = new int[] {0,1,2};
		}

		/*//FOLLOWING CYCLES THROUGH VO'S TO SEE IF CURRENT VELY CAUSES COLLISION (IF COLLISION IN FIRST VO, BREAKS TO REDUCE COST)
		//AFTER WILL FIND ALL OVERLAP POINTS, ADD THEM TO LIST, THEN SEARCH LIST FOR 2 CLOSEST POINTS FOR NEW VELY CALCULATION

		Vector3 A3 = Vector3.zero;
		Vector3 B3 = Vector3.zero;
		Vector3 C3 = Vector3.zero;
		Vector3 P3 = Vector3.zero;
		//Essentially for each VO
		for (int VO = 0; VO < otherAgentList.Count; VO++) 
		{
			//For each
			for (int j = 0; j < 3; j++) {
				if (j == 0) {
					A3 = VOPointsList [VO * 3 + j];
				}
				if (j == 1) {	
					B3 = VOPointsList [VO * 3 + j];					
				}
				if (j == 2) {
					C3 = VOPointsList [VO * 3 + j];
				}
			}
			Vector2 A2 = new Vector2 (A3.x, A3.z);
			Vector2 B2 = new Vector2 (B3.x, B3.z);
			Vector2 C2 = new Vector2 (C3.x, C3.z);
			Vector2 P2 = new Vector2 (objectA.transform.position.x + velyA.x, objectA.transform.position.z + velyA.z);

			if (checkIfCurrentVelyCollides (A2, B2, C2, P2)) 
			{
				//FOLLOWING CHECKS FOR CROSSOVER POINTS OF VO'S
				
				//For however many triangles there are
				int numOfVOs = VOPointsList.Count / 3;
				for (int i = 0; i < numOfVOs; i ++) {
					//check against every other triangle (last needs no checks, second last only needs 1 etc due to 'multiples of checks')
					for (int j = 0; j < numOfVOs - (i + 1); j++) {
						//Check point AB1 against AB2
						checkIntersect (VOPointsList [i * 3], VOPointsList [i * 3 + 1], VOPointsList [(j + 1) * 3], VOPointsList [(j + 1) * 3 + 1]);
						//Check point AB1 against BC2
						checkIntersect (VOPointsList [i * 3], VOPointsList [i * 3 + 1], VOPointsList [((j + 1) * 3) + 2], VOPointsList [((j + 1) * 3 )+ 1]);
						//Check point AB1 against AC2
						checkIntersect (VOPointsList [i * 3], VOPointsList [i * 3 + 1], VOPointsList [(j + 1) * 3], VOPointsList [((j + 1) * 3) + 2]);
						//Check point BC1 against AB2
						checkIntersect (VOPointsList [i * 3 + 1], VOPointsList [i * 3 + 2], VOPointsList [(j + 1) * 3], VOPointsList [(j + 1) * 3 + 1]);
						//Check point BC1 against BC2
						checkIntersect (VOPointsList [i * 3 + 1], VOPointsList [i * 3 + 2], VOPointsList [(j + 1) * 3 + 1], VOPointsList [(j + 1) * 3 + 2]);
						//Check point BC1 against AC2
						checkIntersect (VOPointsList [i * 3 + 1], VOPointsList [i * 3 + 2], VOPointsList [(j + 1) * 3], VOPointsList [(j + 1) * 3 + 2]);
						//Check point AC1 against AB2
						checkIntersect (VOPointsList [i * 3], VOPointsList [i * 3 + 2], VOPointsList [(j + 1) * 3], VOPointsList [(j + 1) * 3 + 1]);
						//Check point AC1 against BC2
						checkIntersect (VOPointsList [i * 3], VOPointsList [i * 3 + 2], VOPointsList [(j + 1) * 3 + 1], VOPointsList [(j + 1) * 3 + 2]);
						//Check point AC1 against AC2
						checkIntersect (VOPointsList [i * 3], VOPointsList [i * 3 + 2], VOPointsList [(j + 1) * 3], VOPointsList [(j + 1) * 3 + 2]);
						//i*3 + j
					}
					
				}
				
				VOPointsList.AddRange (CrossOverPointsList);
				
				Vector3[] closestPoints = findClosestVOPoints (VOPointsList);
				
				closestPointA = closestPoints[0];
				closestPointB = closestPoints[1];

				Vector3 nonCollisionVely = ClosestPointToLine (closestPoints [0], closestPoints [1], objectA.transform.position) 
										   - objectA.transform.position;
				nonCollisionVely.y = 0;
				
				
				velyA = nonCollisionVely;
				break;

			}
			else 				
			{
				velyA = new Vector3 (2, 0, 2);
			}

		}



		/*for (int i = 0; i < instantatedCubes.Count; i++) 
		{
			Object.Destroy (instantatedCubes [i]);
		}

		instantatedCubes.Clear();
	
		for (int i = 0; i < VOPointsList.Count; i++) 
		{
			instantatedCubes.Add ((GameObject)Instantiate (testCube, VOPointsList[i], Quaternion.identity));
		}

		for (int i = 0; i < instantatedCubes.Count; i++) 
		{
			instantatedCubes[i].name = "VO Cube" + i;
		}*/
	
		//*/

	}	

	void FixedUpdate()
	{

	}


	bool checkIfCurrentVelyCollides(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
	{
		//Using the code example from http://www.blackpawn.com/texts/pointinpoly/default.html
		// Compute vectors        
		Vector2 v0 = C - A;
		Vector2 v1 = B - A;
		Vector2 v2 = P - A;			
		// Compute dot products
		float dot00 = Vector2.Dot(v0, v0);
		float dot01 = Vector2.Dot(v0, v1);
		float dot02 = Vector2.Dot(v0, v2);
		float dot11 = Vector2.Dot(v1, v1);
		float dot12 = Vector2.Dot(v1, v2);			
		// Compute barycentric coordinates
		float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
		float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
		float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
		// Check if point is in triangle
		return (u >= 0) && (v >= 0) && (u + v < 1);
	}


	void checkIntersect(Vector3 pA1_3,Vector3 pA2_3,Vector3 pB1_3,Vector3 pB2_3)
	{
		Vector2 pA1 = new Vector2(pA1_3.x, pA1_3.z);
		Vector2 pA2 = new Vector2(pA2_3.x, pA2_3.z);
		Vector2 pB1 = new Vector2(pB1_3.x, pB1_3.z);
		Vector2 pB2 = new Vector2(pB2_3.x, pB2_3.z);


		//Intersect check code here
		//http://www.wyrmtale.com/blog/2013/115/2d-line-intersection-in-c
		
		//pA1.transform.position
		// Get A,B,C of first line - points : ps1 to pe1
		float A1 = pA1.y - pA2.y;
		float B1 = pA2.x - pA1.x;
		float C1 = A1 * pA2.x + B1 * pA2.y;
		
		// Get A,B,C of second line - points : ps2 to pe2
		float A2 = pB1.y - pB2.y;
		float B2 = pB2.x - pB1.x;
		float C2 = A2 * pB2.x + B2 * pB2.y;
		
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
			Vector3 intersect = new Vector3 ((B2 * C1 - B1 * C2) / delta, 0.5f, (A1 * C2 - A2 * C1) / delta);
			if(intersect.x >= checkLessThan(pA1.x, pA2.x) && intersect.x <= checkGreaterThan(pA1.x, pA2.x)&& 
			   intersect.z >= checkLessThan(pA1.y, pA2.y) && intersect.z <= checkGreaterThan(pA1.y, pA2.y))
			{
				CrossOverPointsList.Add (intersect);
			}
		}
	}

	//used in the checkIntersect script to sanity check crossover points
	float checkGreaterThan(float A, float B)
	{
		if (A > B) 
		{
			return A;
		}
		return B;

	}

	float checkLessThan(float A, float B)
	{
		if (A > B) 
		{
			return B;
		}
		return A;
	}

	Vector3[] findClosestVOPoints (List<Vector3> VOPoints)
	{

		//FOR ALL POINTS
		//First makes first checked points 'CLOSEST POINT'  (closestPointA)
		//Then after this, if current point is closer than closestPointA, make current Point closestPointA 
		//and make closestPointA = 'SECOND CLOSEST POINT' (closestPointB)
		//ELSE if current point is not closer than A, but closer than B, make current point closestPointB
		//ELSE disregard current point

		Vector3[] closestVOPoints = new Vector3[2];
		float closestPointA = 0, closestPointB = 0, distance;
		for (int i = 0; i < VOPoints.Count; i++) {
			distance = Vector3.Distance (VOPoints [i], objectA.transform.position);
			//pre loads first 2 points for comparisons
			if (i == 0) {
				//CLOSEST POINT
				closestPointA = distance;
				closestVOPoints [0] = VOPoints [i];
			} else if (i == 1) {
				if (distance > closestPointA) {
					//SECOND CLOSEST POINT
					closestPointB = distance;
					closestVOPoints [1] = VOPoints [i];
				} else {
					//CLOSEST POINT
					closestPointB = closestPointA; 
					closestPointA = distance;
					closestVOPoints [0] = VOPoints [i];
				}
			} else {
				if (distance < closestPointA) {
					//CLOSEST POINT
					closestPointB = closestPointA;
					closestPointA = distance;
					closestVOPoints [0] = VOPoints [i];
				} else if (distance < closestPointB) {
					//SECOND CLOSEST POINT
					closestPointB = distance;
					closestVOPoints [1] = VOPoints [i];
				}
			}
		}

		return closestVOPoints;		
	}

	//Function borrowed from answer here... http://answers.unity3d.com/questions/463700/find-closest-point-to-player-input-within-2d-shape.html
	static Vector3 ClosestPointToLine(Vector3 _lineStartPoint, Vector3 _lineEndPoint, Vector3 _testPoint) 
	{
		Vector3 pointTowardStart = _testPoint - _lineStartPoint;
		Vector3 startTowardEnd = (_lineEndPoint - _lineStartPoint).normalized;
		
		float lengthOfLine = Vector3.Distance(_lineStartPoint, _lineEndPoint);
		float dotProduct = Vector3.Dot(startTowardEnd, pointTowardStart);
		
		if (dotProduct <= 0) {
			return _lineStartPoint;
		}
		
		if (dotProduct >= lengthOfLine) {
			return _lineEndPoint;
		}
		
		Vector3 thirdVector = startTowardEnd * dotProduct;
		
		Vector3 closestPointOnLine = _lineStartPoint + thirdVector;
		
		return closestPointOnLine;
	}


	
	/*COMMENTS TO REMIND PETE WHATS HES DOING
	 * can get points of each VO
	 * current plan is to, after finding all VO's find position of all crossovers
	 * keep a list of all points (3 for each VO, and their crossovers)
	 * find closest two points to 'this' object
	 * find closest point on the line between these two using method here ... http://answers.unity3d.com/questions/463700/find-closest-point-to-player-input-within-2d-shape.html
	 */

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine (objectA.transform.position, objectA.transform.position + objAVely);
		Gizmos.color = Color.red;
		Gizmos.DrawLine (objectB.transform.position, objectB.transform.position + objBVely);

	}


	//For checking average FPS
	void OnApplicationQuit()
	{
		Debug.Log (Time.frameCount);
		Debug.Log (Time.timeSinceLevelLoad);
	}



}
