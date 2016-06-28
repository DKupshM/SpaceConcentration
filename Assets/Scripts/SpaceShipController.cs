using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SpaceShipController : MonoBehaviour
{
	//Speed
	public Vector2 speedLimits = new Vector2 (30, 400);

	//Sensitivity
	public float impulseSensitivity = 500f;
	public float turnSensitivity = 60f;
	public float xSensitivity = 5f;
	public float ySensitivity = 5f;

	private Vector3 rot;

	private float desiredImpulseInput = 0f;
	private Vector3 impulse = Vector3.zero;
	private float desiredImpulse = 0f;
	private Vector3 actualImpulse = Vector3.zero;
	private float maxImpulseChange = 100f;

	public float warpSpeed;
	public Vector2 goWarpSpeedLimits = new Vector2 (30, 100);
	public GameObject warpEffect;
	private bool warp = false;
	private bool readyToWarp = false;
	private AsyncOperation load;

	void Start ()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FixedUpdate ()
	{
		if (!warp) {
			Rotate ();
			Move ();
			CheckWarp ();
		} else {
			GoToWarp ();
		}
	}

	private void CheckWarp ()
	{
		if (Input.GetButton ("Warp") && impulse.z >= goWarpSpeedLimits.x && impulse.z <= goWarpSpeedLimits.y) {
			warp = true;
			StartCoroutine (LoadWarp ());
		}
	}

	private IEnumerator LoadWarp ()
	{
		load = SceneManager.LoadSceneAsync ("Warp");
		load.allowSceneActivation = false;
		warpEffect.SetActive (true);
		yield return new WaitForSeconds (10);
		readyToWarp = true;

	}

	private void GoToWarp ()
	{
		//If not at correct Speed go gradually toward it
		if (impulse.z < warpSpeed) {
			desiredImpulse += impulseSensitivity * Time.deltaTime;
			impulse = new Vector3 (0, 0, Mathf.Clamp (desiredImpulse, goWarpSpeedLimits.x, warpSpeed));
		}

		// Smooth the Movement
		if (Vector3.Distance (impulse, actualImpulse) < maxImpulseChange * Time.deltaTime) {
			actualImpulse = impulse;
		} else {
			actualImpulse += (impulse - actualImpulse).normalized * maxImpulseChange * Time.deltaTime;
		}

		//Move Ship
		transform.Translate ((transform.rotation * actualImpulse / 10f) * Time.deltaTime, Space.World);

		if (readyToWarp && impulse.z == warpSpeed) {
			load.allowSceneActivation = true;
		}
	}

	private void Move ()
	{
		// Get the Input
		desiredImpulse += Input.GetAxis ("Thrust") * impulseSensitivity * Time.deltaTime;
		impulse = new Vector3 (0, 0, Mathf.Clamp (desiredImpulse, speedLimits.x, speedLimits.y));

		// Smooth the Movement
		if (Vector3.Distance (impulse, actualImpulse) < maxImpulseChange * Time.deltaTime) {
			actualImpulse = impulse;
		} else {
			actualImpulse += (impulse - actualImpulse).normalized * maxImpulseChange * Time.deltaTime;
		}

		//Move Ship
		transform.Translate ((transform.rotation * actualImpulse / 10f) * Time.deltaTime, Space.World);
	}

	private void Rotate ()
	{
		//Pitch  or Up and Down
		rot.x = -Input.GetAxis ("Mouse Y") * ySensitivity;
	
		//Yaw or Left and Right
		rot.y = Input.GetAxis ("Mouse X") * xSensitivity;

		// Roll or CW and CCW
		rot.z = Input.GetAxis ("Horizontal") * turnSensitivity * Time.deltaTime;

		//Rotate Ship
		transform.Rotate (rot, Space.Self);
	}
}
