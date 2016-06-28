using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(SkyboxGenerator))]
public class SkyBoxEditor : Editor
{

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		Generator gen = (Generator)target;

		if (GUILayout.Button ("Generate Skybox")) {
			gen.Generate ();
		}
		if (GUILayout.Button ("Clear Skybox")) {
			gen.Clear ();
		}
	}
}
