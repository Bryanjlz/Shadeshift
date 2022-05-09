using UnityEngine;

class DarkObject : LightReactor {

	[SerializeField] Collider2D target;
	[SerializeField] SpriteRenderer outline;
	[SerializeField] LineRenderer line;
	[SerializeField] float fadeTime = 0.25f;
	float elapsedTime = 0f;

	override internal void OnLightEnter() {
		target.enabled = true;
	}

	override internal void OnLightExit() {
		target.enabled = false;
	}

	void Update() {
		elapsedTime = Mathf.Clamp(Lit ? elapsedTime + Time.deltaTime : elapsedTime - Time.deltaTime, 0f, fadeTime);
		if (outline != null) {
			outline.color = new Color(outline.color.r, outline.color.b, outline.color.g, elapsedTime / fadeTime);
		}
		if (line != null) {
			line.startColor = new Color(line.startColor.r, line.startColor.g, line.startColor.b, elapsedTime / fadeTime);
			line.endColor = line.startColor;
		}
	}

	// void FixedUpdate() {
	// 	Vector2 ray = collider.transform.position - transform.position;
	// 	float mag = ray.magnitude;
	// 	RaycastHit2D hit = Physics2D.Raycast(transform.position, ray, mag, shadowMask);
	// }
}
