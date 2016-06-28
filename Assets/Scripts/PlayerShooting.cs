using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{

	public float cooldown = 0f;
	public float fireRate = 0f;

	public bool isFiring = false;

	public Transform[] firePoints;

	public GameObject laserPrefab;
	public Transform laserParent;

	public AudioSource soundFireFX;

	void Update ()
	{
		CheckInput ();
		cooldown -= Time.deltaTime;
		if (isFiring && cooldown <= 0) {
			Fire ();
		}
	}

	private void Fire ()
	{
		if (soundFireFX != null)
			soundFireFX.Play ();
		foreach (Transform t in firePoints) {
			GameObject trans = Instantiate (laserPrefab, t.position, t.rotation) as GameObject;
			if (laserParent) {
				trans.transform.parent = laserParent;
			}
		}
		cooldown = fireRate;
	}

	private void CheckInput ()
	{
		if (Input.GetButton ("Fire1")) {
			isFiring = true;
		} else {
			isFiring = false;
		}
	}
}
