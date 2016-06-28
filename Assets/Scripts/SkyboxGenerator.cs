using UnityEngine;
using CoherentNoise.Generation;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Generation.Modification;
using CoherentNoise.Generation.Displacement;
using CoherentNoise.Texturing;
using UnityEngine.UI;
using System.Collections;

public class SkyboxGenerator : MonoBehaviour, Generator
{
	public int seed = 1;
	public bool randomize = true;

	public float frequency = 4f;
	public float persistance = .4f;

	public int size;
	public RawImage image;

	public Color firstGrad;
	public Color secondGrad;


	private CoherentNoise.Generator noise;
	private Texture tex;

	void Start ()
	{
		Generate ();
	}

	public void Generate ()
	{
		if (randomize) {
			seed = Random.Range (0, Mathf.Abs (Random.seed));
			Debug.Log ("Using " + seed + " to make skybox");
		}

		noise = GetNoise (seed);

		System.Func<float, float, Color> func = GetColor;
		tex = TextureMaker.Make (size, size, func, TextureFormat.RGB24);
		image.texture = tex;
	}

	private CoherentNoise.Generator GetNoise (int seed)
	{
		BillowNoise noise = new BillowNoise (seed);
		Turbulence b = new Turbulence (noise, seed);
		return b;
	}


	private Color GetColor (float first, float second)
	{
		float value = noise.GetValue (first, second, 0);
		return Color.Lerp (firstGrad, secondGrad, .5f + value / 2);
		
	}

	public void Clear ()
	{

	}
}
