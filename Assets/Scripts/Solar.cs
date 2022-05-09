using UnityEngine;

class Solar : LightReactor {

	[SerializeField]
	Transform[] gates;

	[Header("Tuning")]
	[SerializeField] AnimationCurve powerCurve;
	[SerializeField] Color unpowered, powered, poweredPlate, blinkColor;
	[SerializeField] float chargeTime = 3f;
	[SerializeField] SpriteRenderer sprite;
	[SerializeField] SpriteRenderer plate;
	[SerializeField] SpriteRenderer covering;
	float currentCharge = 0f;
	bool active => currentCharge >= chargeTime;
	int whiteFrames = -1;

	// void Reset() {
	// 	sprite = GetComponent<SpriteRenderer>();
	// }

	void Update() {
		if (whiteFrames == -2) return;
		if (Lit) currentCharge = Mathf.Clamp(currentCharge + Time.deltaTime, 0f, chargeTime);
		sprite.color = Color.Lerp(unpowered, powered, powerCurve.Evaluate(currentCharge / chargeTime));
		// sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);

		if (whiteFrames == -1 && active) {
			covering.enabled = false;
			Sound.self.PlayGate();
			foreach (Transform gate in gates) {
				foreach (Transform child in gate) {
					child.gameObject.SetActive(!child.gameObject.activeSelf);
				}
			}
			sprite.color = blinkColor;
			// plate.color = blinkColor;
			whiteFrames = 10;
			return;
		}
		if (whiteFrames == 0) {
			sprite.color = powered;
			plate.color = poweredPlate;
			whiteFrames = -2;
		}
		if (whiteFrames > 0) whiteFrames--;
	}

	override internal void OnLightEnter() {

	}

	override internal void OnLightExit() {
		if (!active) currentCharge = 0f;
	}
}
