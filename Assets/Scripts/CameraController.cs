using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform target;
	public Vector3 distance = new Vector3 (10, 5, 0);
	public float positionDamping = 1.0f;
	public float rotationDamping = 2.0f;

	void LateUpdate ()
	{
		if (!target)
			return;
		transform.position = Vector3.Lerp (transform.position, target.position + (target.rotation * distance), positionDamping * Time.deltaTime);

		transform.rotation = Quaternion.LookRotation (target.position - transform.position, target.up);
	}
}