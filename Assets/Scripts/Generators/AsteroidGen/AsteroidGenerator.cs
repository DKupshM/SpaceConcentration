﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[RequireComponent (typeof(MeshCollider))]
public class AsteroidGenerator : MonoBehaviour, Generator
{

	private struct TriangleIndices
	{
		public int v1;
		public int v2;
		public int v3;

		public TriangleIndices (int v1, int v2, int v3)
		{
			this.v1 = v1;
			this.v2 = v2;
			this.v3 = v3;
		}
	}

	public float rad;
	public int detail;

	private float timeX = 0f;
	private float timeY = 0f;
	private float timeZ = 0f;
	private Vector3[] baseVertices;
	private Mesh mesh;
	private Perlin noise;

	public void Generate ()
	{
		timeX = 0;
		timeY = 0;
		timeZ = 0;
		Create (rad, detail);
		CrumpleMesh ();
		GetComponent<MeshCollider> ().sharedMesh = mesh;
	}

	private int getMiddlePoint (int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
	{
		// first check if we have it already
		bool firstIsSmaller = p1 < p2;
		long smallerIndex = firstIsSmaller ? p1 : p2;
		long greaterIndex = firstIsSmaller ? p2 : p1;
		long key = (smallerIndex << 32) + greaterIndex;

		int ret;
		if (cache.TryGetValue (key, out ret)) {
			return ret;
		}
		// not in cache, calculate it
		Vector3 point1 = vertices [p1];
		Vector3 point2 = vertices [p2];
		Vector3 middle = new Vector3 ((point1.x + point2.x) / 2f, (point1.y + point2.y) / 2f, (point1.z + point2.z) / 2f);

		// Add Vertex on point of unit sphere
		int i = vertices.Count;
		vertices.Add (middle.normalized * radius);

		//Store it and return index
		cache.Add (key, i);
		return i;
	}

	public void Create (float radius, int recursionLevel)
	{
		mesh = new Mesh ();
		if (GetComponent<MeshFilter> ()) {
			GetComponent<MeshFilter> ().sharedMesh = mesh;
		} else {
			this.gameObject.AddComponent<MeshFilter> ().sharedMesh = mesh;
		}

		List<Vector3> vertList = new List<Vector3> ();
		Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int> ();

		// create 12 vertices of a icosahedron
		float t = (1f + Mathf.Sqrt (5f)) / 2f;

		vertList.Add (new Vector3 (-1f, t, 0f).normalized * radius);
		vertList.Add (new Vector3 (1f, t, 0f).normalized * radius);
		vertList.Add (new Vector3 (-1f, -t, 0f).normalized * radius);
		vertList.Add (new Vector3 (1f, -t, 0f).normalized * radius);

		vertList.Add (new Vector3 (0f, -1f, t).normalized * radius);
		vertList.Add (new Vector3 (0f, 1f, t).normalized * radius);
		vertList.Add (new Vector3 (0f, -1f, -t).normalized * radius);
		vertList.Add (new Vector3 (0f, 1f, -t).normalized * radius);

		vertList.Add (new Vector3 (t, 0f, -1f).normalized * radius);
		vertList.Add (new Vector3 (t, 0f, 1f).normalized * radius);
		vertList.Add (new Vector3 (-t, 0f, -1f).normalized * radius);
		vertList.Add (new Vector3 (-t, 0f, 1f).normalized * radius);
		// create 20 triangles of the icosahedron
		List<TriangleIndices> faces = new List<TriangleIndices> ();

		// 5 faces around point 0
		faces.Add (new TriangleIndices (0, 11, 5));
		faces.Add (new TriangleIndices (0, 5, 1));
		faces.Add (new TriangleIndices (0, 1, 7));
		faces.Add (new TriangleIndices (0, 7, 10));
		faces.Add (new TriangleIndices (0, 10, 11));

		// 5 adjacent faces
		faces.Add (new TriangleIndices (1, 5, 9));
		faces.Add (new TriangleIndices (5, 11, 4));
		faces.Add (new TriangleIndices (11, 10, 2));
		faces.Add (new TriangleIndices (10, 7, 6));
		faces.Add (new TriangleIndices (7, 1, 8));

		// 5 faces around point 3
		faces.Add (new TriangleIndices (3, 9, 4));
		faces.Add (new TriangleIndices (3, 4, 2));
		faces.Add (new TriangleIndices (3, 2, 6));
		faces.Add (new TriangleIndices (3, 6, 8));
		faces.Add (new TriangleIndices (3, 8, 9));

		// 5 adjacent faces
		faces.Add (new TriangleIndices (4, 9, 5));
		faces.Add (new TriangleIndices (2, 4, 11));
		faces.Add (new TriangleIndices (6, 2, 10));
		faces.Add (new TriangleIndices (8, 6, 7));
		faces.Add (new TriangleIndices (9, 8, 1));

		// refine triangles
		for (int i = 0; i < recursionLevel; i++) {
			List<TriangleIndices> faces2 = new List<TriangleIndices> ();
			foreach (var tri in faces) {
				// replace triangle by 4 triangles
				int a = getMiddlePoint (tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, radius);
				int b = getMiddlePoint (tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, radius);
				int c = getMiddlePoint (tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, radius);

				faces2.Add (new TriangleIndices (tri.v1, a, c));
				faces2.Add (new TriangleIndices (tri.v2, b, a));
				faces2.Add (new TriangleIndices (tri.v3, c, b));
				faces2.Add (new TriangleIndices (a, b, c));
			}
			faces = faces2;
		}

		mesh.vertices = vertList.ToArray ();

		List<int> triList = new List<int> ();
		for (int i = 0; i < faces.Count; i++) {
			triList.Add (faces [i].v1);
			triList.Add (faces [i].v2);
			triList.Add (faces [i].v3);
		}

		mesh.triangles = triList.ToArray ();
		mesh.uv = new Vector2[mesh.vertices.Length];
		Vector3[] normales = new Vector3[vertList.Count];
		for (int i = 0; i < normales.Length; i++) {
			normales [i] = vertList [i].normalized;
		}

		mesh.normals = normales;
		mesh.RecalculateBounds ();
		mesh.Optimize ();
	}

	public void CrumpleMesh ()
	{
		noise = new Perlin ();
	
		if (baseVertices == null)
			baseVertices = mesh.vertices;
		
		Vector3[] vertices = new Vector3[baseVertices.Length];
	
		timeX += 0.1365143f;
		timeY += 1.21688f;
		timeZ += 2.5564f;

		for (var i = 0; i < vertices.Length; i++) {
			Vector3 vertex = baseVertices [i];
				
			vertex.x += noise.Noise (timeX + vertex.x, timeX + vertex.y, timeX + vertex.z) * rad;
			vertex.y += noise.Noise (timeY + vertex.x, timeY + vertex.y, timeY + vertex.z) * rad;
			vertex.z += noise.Noise (timeZ + vertex.x, timeZ + vertex.y, timeZ + vertex.z) * rad;
		
			vertices [i] = vertex;
		}
	
		mesh.vertices = vertices;
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();
		mesh.Optimize ();
	}

	public void Clear ()
	{
		DestroyImmediate (this.gameObject);
	}
}