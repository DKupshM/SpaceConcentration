using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	public float health = 100f;
	public GameObject destroyFX;

	public void TakeDamage (float damage)
	{
		health -= damage;
		if (health <= 0) {
			Instantiate (destroyFX, transform.position, transform.rotation);
			Destroy (this.gameObject);
		}
	}
}
