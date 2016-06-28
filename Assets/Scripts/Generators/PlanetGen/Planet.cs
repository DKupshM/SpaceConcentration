using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour, Generator
{

	public void Generate ()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild (i);
			child.position = transform.position;
			child.rotation = transform.rotation;
			child.localScale = Vector3.one;
			if (child.GetComponent<PlanetGenerator> () != null) {
				PlanetGenerator gen = child.GetComponent<PlanetGenerator> ();
				gen.Generate ();
			}
		}
	}

	public void Clear ()
	{
		DestroyImmediate (this.gameObject);
	}
}