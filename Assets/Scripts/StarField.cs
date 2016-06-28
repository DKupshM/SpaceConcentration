using UnityEngine;
using System.Collections;

public class StarField : MonoBehaviour
{
	public int starsMax = 100;
	public float starSize = 1;
	public float starDistance = 10;
	public float starClipDistance = 1;

	private float starDistanceSqr;
	private float starClipDistanceSqr;
	private ParticleSystem.Particle[] points;

	void Start ()
	{
		starDistanceSqr = starDistance * starDistance;
		starClipDistanceSqr = starClipDistance * starClipDistance;
	}

 
	private void CreateStars ()
	{
		points = new ParticleSystem.Particle[starsMax];
 
		for (int i = 0; i < starsMax; i++) {
			points [i].position = Random.insideUnitSphere * starDistance + transform.position;
			points [i].startColor = new Color (1, 1, 1, 1);
			points [i].startSize = starSize;
		}
	}

	void Update ()
	{
		if (points == null) {
			CreateStars ();
		}

		for (int i = 0; i < starsMax; i++) {
 
			if ((points [i].position - transform.position).sqrMagnitude > starDistanceSqr) {
				points [i].position = Random.insideUnitSphere.normalized * starDistance + transform.position;
			}
			if ((points [i].position - transform.position).sqrMagnitude <= starClipDistanceSqr) {
				float percent = (points [i].position - transform.position).sqrMagnitude / starClipDistanceSqr;
				points [i].startColor = new Color (1, 1, 1, percent);
				points [i].startSize = percent * starSize;
			}
		}

		GetComponent<ParticleSystem> ().SetParticles (points, points.Length);
	}
}
