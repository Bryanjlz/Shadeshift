using UnityEngine;

class EyePivot : MonoBehaviour {

	Camera mainCamera;
	[SerializeField] float reactRange = 10f;
	[SerializeField] Vector2 motionRange = new Vector2(0.5f, 0.2f);

	void OnEnable() {
		mainCamera = Camera.main;
	}

	void LateUpdate() {
		Vector2 ray = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		ray = Vector2.ClampMagnitude(ray, reactRange) / reactRange * motionRange;
		transform.localPosition = ray;
	}
}
