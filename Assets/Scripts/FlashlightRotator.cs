using UnityEngine;

class FlashlightRotator : MonoBehaviour {

	[SerializeField] float turnSpeed = 360f;
	Camera mainCamera;

	void OnEnable() {
		mainCamera = Camera.main;
	}

	void LateUpdate() {
		Vector2 ray = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);
		float x = Vector2.Dot(ray.normalized, transform.right);
		transform.Rotate(0f, 0f, x * turnSpeed * Time.deltaTime);
	}
}
