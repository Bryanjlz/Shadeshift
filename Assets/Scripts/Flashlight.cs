using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class Flashlight : MonoBehaviour {

	struct CastInfo {
		internal Vector2 point;
		internal float distance;
		internal float angle;
		internal Vector2 normal;

		internal CastInfo(Vector2 point, float distance, float angle, Vector2 normal) {
			this.point = point;
			this.distance = distance;
			this.angle = angle;
			this.normal = normal;
		}
	}

	struct Edge {
		internal Vector2? a;
		internal Vector2? b;

		internal Edge(Vector2? a, Vector2? b) {
			this.a = a;
			this.b = b;
		}
	}

	[Header("Tuning")]
	[SerializeField] float viewRadius = 12f;
	[SerializeField] [Range(0f, 120f)] float viewAngle = 60f;
	[SerializeField] int edgeIterations = 2;
	[SerializeField] float edgeThreshold = 0.5f;

	[Header("Setup")]
	[SerializeField] LayerMask shadowMask;
	[SerializeField] MeshFilter meshFilter;
	[SerializeField] PolygonCollider2D lightCollider;
	[SerializeField] Transform aura;
	[SerializeField] MeshRenderer meshRenderer;
	Mesh viewMesh;

	HashSet<LightReactor> litReactors = new HashSet<LightReactor>();
	int bufferFrames = 1;

	void Reset() {
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		lightCollider = GetComponent<PolygonCollider2D>();
	}

	void OnEnable() {
		bufferFrames = 1;
		if (meshFilter.mesh == null) {
			viewMesh = new Mesh();
			meshFilter.mesh = viewMesh;
		} else {
			viewMesh = meshFilter.mesh;
		}

		float x = viewRadius * Mathf.Tan(viewAngle / 2f * Mathf.Deg2Rad);
		lightCollider.points = new Vector2[] {
			Vector2.zero,
			new Vector2(-x, viewRadius),
			new Vector2(x, viewRadius)
		};
	}

	void OnDisable() {
		foreach (LightReactor reactor in litReactors) {
			if (reactor != null) reactor.ParseLightExit(this);
		}
		litReactors.Clear();
		meshRenderer.enabled = false;
	}

	void FixedUpdate() {
		UpdateMesh();
	}

	void LateUpdate() {
		UpdateMesh();
		if (bufferFrames > 0) {
			bufferFrames--;
		} else if (bufferFrames == 0) {
			meshRenderer.enabled = true;
			bufferFrames--;
		}
	}

	void OnTriggerStay2D(Collider2D collider) {
		LightReactor reactor = collider.GetComponent<LightReactor>();
		if (reactor == null) return;
		CheckReactor(reactor);
	}

	void OnTriggerExit2D(Collider2D collider) {
		LightReactor reactor = collider.GetComponent<LightReactor>();
		if (reactor != null) RemoveReactor(reactor);
	}

	void CheckReactor(LightReactor reactor) {
		Vector2 ray = reactor.transform.position - transform.position;
		float mag = ray.magnitude;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, ray, mag, shadowMask);
		// float deltaAngle = Mathf.Atan2(ray.y, ray.x) * Mathf.Rad2Deg - 90f;
		// deltaAngle -= transform.eulerAngles.z;
		// if (deltaAngle < 0f) deltaAngle += 360f;
		// print($"{deltaAngle} {viewAngle / 2f}");
		// (Mathf.Abs(deltaAngle) > viewAngle / 2f && mag > aura.localScale.x)
		if (mag > viewRadius || (hit.collider != null && hit.collider.gameObject != reactor.gameObject && !hit.collider.transform.IsChildOf(reactor.transform))) {
			// Debug.DrawRay(transform.position, ray, Color.red);
			RemoveReactor(reactor);
		} else {
			// Debug.DrawRay(transform.position, ray, Color.blue);
			AddReactor(reactor);
		}
	}

	void RemoveReactor(LightReactor reactor) {
		if (bufferFrames > 0) return;
		reactor.ParseLightExit(this);
		litReactors.Remove(reactor);
	}

	void AddReactor(LightReactor reactor) {
		if (bufferFrames > 0) return;
		reactor.ParseLightEnter(this);
		litReactors.Add(reactor);
	}

	void UpdateMesh() {
		int stepCount = (int)(viewAngle * 2f);
		float stepAngle = viewAngle / stepCount;
		List<Vector2> viewPoints = new List<Vector2>();
		float angle = transform.eulerAngles.z - viewAngle * 0.5f;
		CastInfo prevCast = ViewCast(angle);
		viewPoints.Add(prevCast.point);
		for (int i = 1; i <= stepCount; i++) {
			angle = transform.eulerAngles.z - viewAngle * 0.5f + stepAngle * i;
			CastInfo cast = ViewCast(angle);
			if (Mathf.Abs(prevCast.distance - cast.distance) > edgeThreshold || prevCast.normal != cast.normal) {
				Edge edge = FindEdge(prevCast, cast);
				if (edge.a != null) {
					viewPoints.Add((Vector2)edge.a);
				}
				if (edge.b != null) {
					viewPoints.Add((Vector2)edge.b);
				}
			}
			viewPoints.Add(cast.point);
			prevCast = cast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount-2) * 3];

		vertices[0] = Vector2.zero;
		for (int i = 0; i < vertexCount - 1; i++) {
			vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
			if (i < vertexCount - 2) {
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}


	Edge FindEdge(CastInfo minCast, CastInfo maxCast) {
		float minAngle = minCast.angle;
		float maxAngle = maxCast.angle;
		Vector2? minPoint = null;
		Vector2? maxPoint = null;
		for (int i = 0; i < edgeIterations; i++) {
			float angle = (minAngle + maxAngle) / 2;
			CastInfo cast = ViewCast(angle);
			if (Mathf.Abs(minCast.distance - cast.distance) < edgeThreshold || minCast.normal != cast.normal) {
				minAngle = angle;
				minPoint = cast.point;
			} else {
				maxAngle = angle;
				maxPoint = cast.point;
			}
		}
		return new Edge(minPoint, maxPoint);
	}

	CastInfo ViewCast(float angle) {
		Vector2 ray = Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.up;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, ray, viewRadius, shadowMask);
		if (hit.normal != Vector2.zero) {
			return new CastInfo(hit.point, hit.distance, angle, hit.normal);
		} else {
			return new CastInfo((Vector2)transform.position + ray * viewRadius, viewRadius, angle, hit.normal);
		}
	}
}
