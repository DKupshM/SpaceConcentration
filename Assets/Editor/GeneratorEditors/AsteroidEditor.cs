using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(AsteroidGenerator))]
class AsteroidEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		Generator gen = (Generator)target;
		if (GUILayout.Button ("Generate new Asteroid")) {
			gen.Generate ();
		}

		if (GUILayout.Button ("Destroy Asteroid")) {
			gen.Clear ();
		}
	}
}
