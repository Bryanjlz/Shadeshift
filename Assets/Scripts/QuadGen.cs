using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

class QuadGen : MonoBehaviour {

	[SerializeField] float horizontalRad;
	[SerializeField] float verticalRad;
	[SerializeField] float chaosRadius = 0.5f;
	[SerializeField] MeshFilter filter;
	[SerializeField] MeshCollider meshCollider;

	Vector2 chaos => Random.insideUnitCircle * chaosRadius;

	void Reset() {
		filter = GetComponent<MeshFilter>();
		meshCollider = GetComponent<MeshCollider>();
	}

	void OnEnable() {
		Vector2[] square = new Vector2[] {
			new Vector2(horizontalRad, verticalRad) + chaos,
			new Vector2(horizontalRad, -verticalRad) + chaos,
			new Vector2(-horizontalRad, -verticalRad) + chaos,
			new Vector2(-horizontalRad, verticalRad) + chaos
		};
		Mesh mesh = new Mesh();
		mesh.vertices = System.Array.ConvertAll<Vector2, Vector3>(square, i => i);
		mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		filter.mesh = mesh;
		meshCollider.sharedMesh = mesh;
	}
}
