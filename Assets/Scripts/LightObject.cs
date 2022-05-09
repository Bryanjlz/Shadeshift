using UnityEngine;

class LightObject : LightReactor {

	[SerializeField] Collider2D target;

	override internal void OnLightEnter() {
		target.enabled = false;
	}

	override internal void OnLightExit() {
		target.enabled = true;
	}
}
