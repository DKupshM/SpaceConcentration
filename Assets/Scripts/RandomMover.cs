using UnityEngine;
using System.Collections;

public class RandomMover : MonoBehaviour
{
	public Vector2 tumble = new Vector2 (1, 90);
	public bool rotate = true;
	public Vector2 speed = new Vector2 (1, 5);
	public bool move = true;

	void Start ()
	{
		if (rotate) {
			GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * Random.Range (tumble.x, tumble.y);
		}
		if (move) {
			GetComponent<Rigidbody> ().velocity = Random.insideUnitSphere * Random.Range (speed.x, speed.y);
		}
	}

}
