using UnityEngine;
using UnityEngine.SceneManagement;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Texturing;
using System.Collections;
using System.Collections.Generic;

public class SpaceField : MonoBehaviour, Generator
{
	public BoxCollider spaceBounds;


	public int seed = 1;
	public bool randomizeSeed = true;
	public int scalar = 5;
	public bool generateOnStart = false;

	public GameObject asteroid;
	public Vector2 asteroidNoise = new Vector2 (.6f, 1f);
	public bool generateAsteroids = true;

	public GameObject planet;
	public Vector2 planetNoise = new Vector2 (-1f, -1f);
	public bool generatePlanets = true;

	private PinkNoise noise;
	private List<Collider> noGenSpace;

	void Start ()
	{
		if (generateOnStart) {
			Generate ();
		}
	}

	public void Generate ()
	{
		//Make Seed For Noise
		if (randomizeSeed) {
			seed = Random.seed;
			Debug.Log ("Using " + seed + " as seed for space field");
		}
		//Make Noise
		noise = new PinkNoise (seed);

		//Set Up area
		Clear ();
		SetNoGenArea ();

		//Generate
		StartCoroutine (CreateField ());
		
	}

	private IEnumerator CreateField ()
	{
		int count = 0;
		Vector3 size = spaceBounds.bounds.size;
		Vector3 min = spaceBounds.bounds.min;	
		if (generatePlanets) {
			for (int i = 0; i < size.x; i += scalar) {
				for (int j = 0; j < size.y; j += scalar) {
					for (int k = 0; k < size.z; k += scalar) {
						Vector3 point = new Vector3 (i + min.x, j + min.y, k + min.z);
						if (IsPointValid (point)) {
							float value = noise.GetValue (point.x, point.y, point.z);
							if (value > planetNoise.x && value <= planetNoise.y) {
								GameObject planetObject = Instantiate (planet, point, planet.transform.rotation) as GameObject;
								planetObject.SendMessage ("Generate", SendMessageOptions.DontRequireReceiver);
								if (planetObject.GetComponent<Collider> ()) {
									noGenSpace.Add (planetObject.GetComponent<Collider> ());
								}
								planetObject.transform.SetParent (transform);
								yield return null;
								count++;
							}
						}
					}
				}
			}
			Debug.Log (count + " Planets created");
		}

		if (generateAsteroids) {
			count = 0;
			for (int i = 0; i < size.x; i += scalar) {
				for (int j = 0; j < size.y; j += scalar) {
					for (int k = 0; k < size.z; k += scalar) {
						Vector3 point = new Vector3 (i + min.x, j + min.y, k + min.z);
						if (IsPointValid (point)) {
							float value = noise.GetValue (point.x, point.y, point.z);
							if (value > asteroidNoise.x && value <= asteroidNoise.y) {
								GameObject asteroidObject = Instantiate (asteroid, point, asteroid.transform.rotation) as GameObject;
								asteroidObject.SendMessage ("Generate", SendMessageOptions.DontRequireReceiver);
								if (asteroidObject.GetComponent<Collider> ()) {
									noGenSpace.Add (asteroidObject.GetComponent<Collider> ());
								}
								asteroidObject.transform.SetParent (transform);
								count++;
								if (count % 3 == 0) {
									yield return null;
								}
							}
						}
					}
				}
			}
			Debug.Log (count + " Asteroids created");
		}

		if (generateOnStart) {
			GameObject[] warpObjects = SceneManager.GetSceneByName ("Warp").GetRootGameObjects ();
			foreach (GameObject obj in warpObjects) {
				if (obj) {
					obj.SendMessage ("EnableNextScene", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	private void SetNoGenArea ()
	{
		noGenSpace = new List<Collider> ();
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("NoGenSpace")) {
			if (obj.GetComponent<Collider> ()) {
				noGenSpace.Add (obj.GetComponent<Collider> ());
			}
		}
	}

	private bool IsPointValid (Vector3 point)
	{
		foreach (Collider coll in noGenSpace) {
			if (coll.bounds.Contains (point))
				return false;
		}
		return true;
	}

	public void Clear ()
	{
		while (transform.childCount != 0) {
			DestroyImmediate (transform.GetChild (0).gameObject);
		}
		if (noGenSpace != null) {
			noGenSpace.Clear ();
		}
	}
}