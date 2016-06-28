using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WarpSceneLoader : MonoBehaviour
{
	public string sceneName;
	public Text progressText;

	private AsyncOperation load;
	private GameObject[] objects;


	void Start ()
	{
		StartCoroutine (loadNext ());
	}

	private IEnumerator loadNext ()
	{
		//Delay to allow ship to smooth
		yield return new WaitForSeconds (2);

		load = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		load.allowSceneActivation = false;
		while (load.progress < .9f) {
			yield return null;
		}
		load.allowSceneActivation = true;
		Debug.Log ("Activating Scene");

	}

	public void EnableNextScene ()
	{
		Debug.Log ("Going out of warp");
		GameObject[] objects = SceneManager.GetSceneByName (sceneName).GetRootGameObjects ();
		foreach (GameObject obj in objects) {
			obj.SetActive (true);
		}
		SceneManager.UnloadScene ("Warp");
	}
}
