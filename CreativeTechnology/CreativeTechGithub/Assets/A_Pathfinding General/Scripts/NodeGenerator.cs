using UnityEngine;
using System.Collections;

public class NodeGenerator : MonoBehaviour {

	public int width, height;
	public float spacing;
	public GameObject node;


	// Use this for initialization
	void Awake () {

		FindObjectOfType<Node> ().setCheckRadius (Mathf.Sqrt (((2 * (spacing * spacing)))));
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void buildGrid()
	{
		GameObject nodeHolder = new GameObject();
		nodeHolder.name = "Nodes";
		for (int i = 0; i < width; i++) 
		{
			for (int j = 0; j < height; j++) 
			{
				GameObject obj = (GameObject)Instantiate(node, new Vector3(i*spacing, 0f, j*spacing), Quaternion.identity);
				obj.GetComponent<Node> ().setCheckRadius(Mathf.Sqrt(((2*(spacing * spacing)))));
				obj.transform.parent = nodeHolder.transform;
			}
		}
	}
}
