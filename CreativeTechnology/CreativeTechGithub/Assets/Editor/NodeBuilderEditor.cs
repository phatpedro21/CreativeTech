using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(NodeGenerator))]
public class NodeBuilderEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		NodeGenerator targetScript = (NodeGenerator)target;
		if (GUILayout.Button ("BuildGrid"))
		{
			targetScript.buildGrid ();
		}
	}
}
