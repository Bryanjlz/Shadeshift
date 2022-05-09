using UnityEngine;

class Rotator : MonoBehaviour {

	[SerializeField] float angle = 30f;
	[SerializeField] float timeScale = 0.5f;

	float Hermite(float time) {
		time = Mathf.Clamp(time, 0f, 1f);
		return time * time * (3f - 2f * time);
	}

	void LateUpdate() {
		transform.rotation = Quaternion.AngleAxis(angle / 2f - angle * Hermite(Mathf.PingPong(Time.time * timeScale, 1f)), Vector3.forward);
	}
}
