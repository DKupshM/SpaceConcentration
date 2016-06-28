using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(SpaceField))]
public class SpaceEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		Generator gen = (Generator)target;

		if (GUILayout.Button ("Generate Space Field")) {
			gen.Generate ();
		}
		if (GUILayout.Button ("Clear Space Field")) {
			gen.Clear ();
		}
	}
}
