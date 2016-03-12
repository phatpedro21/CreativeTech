using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NetworkBuild))]
public class PathCheckingEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if(GUILayout.Button("Build Path"))
		{
			var NetworkBuilder = target as NetworkBuild;
			NetworkBuilder.buildPath ();
		}
	}


}