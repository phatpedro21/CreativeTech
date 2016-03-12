using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if(GUILayout.Button("Set Start"))
		{
			var startNode = target as Node;
			if (GameObject.FindGameObjectWithTag ("StartNode"))
			{
				GameObject previousStart = GameObject.FindGameObjectWithTag ("StartNode");
				previousStart.tag = "Node";
			}

				
			startNode.tag = "StartNode";
		}
		if(GUILayout.Button("Set End"))
		{
			var startNode = target as Node;
			if (GameObject.FindGameObjectWithTag ("EndNode"))
			{
				GameObject previousEnd = GameObject.FindGameObjectWithTag ("EndNode");
				previousEnd.tag = "Node";
			}
			startNode.tag = "EndNode";
		}
	}


}
