using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Planet))]
class PlanetEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		Generator planet = (Generator)target;
		if (GUILayout.Button ("Generate new Planet")) {
			planet.Generate ();
		}
		if (GUILayout.Button ("Destroy Planet")) {
			planet.Clear ();
		}
	}
}
