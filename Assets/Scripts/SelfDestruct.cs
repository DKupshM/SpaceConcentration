using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{

	public float selfDestructTime = 1f;

	void Update ()
	{
		selfDestructTime -= Time.deltaTime;
		if (selfDestructTime < 0)
			Destroy (this.gameObject);
	}
}
