using UnityEngine;
using System.Collections;

public class LaserProjectile : MonoBehaviour
{

	public float speed = 2f;
	public float damage = 25f;
	public Transform laserHitFXPrefab;
	
	// Update is called once per frame
	void Update ()
	{
		transform.position += Time.deltaTime * speed * transform.forward;
	}

	void OnCollisionEnter (Collision other)
	{
		other.gameObject.SendMessage ("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		if (other.transform.parent) {
			other.transform.parent.SendMessage ("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
		if (laserHitFXPrefab != null) {
			ContactPoint contact = other.contacts [0];
			Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
			Vector3 pos = contact.point;
			Instantiate (laserHitFXPrefab, pos, rot);
		}
		Destroy (this.gameObject);
	}
}
