using UnityEngine;

class Emotion : MonoBehaviour {

	[SerializeField] SpriteRenderer leftRenderer;
	[SerializeField] SpriteRenderer rightRenderer;
	[SerializeField] Sprite neutral, happy, hurtLeft, hurtRight;

	internal static Emotion self;
	internal bool won;
	internal bool lost;

	void OnEnable() {
		self = this;
	}

	internal void SetNeutral() {
		leftRenderer.sprite = neutral;
		rightRenderer.sprite = neutral;
		won = false;
		lost = false;
	}

	internal void SetHappy() {
		leftRenderer.sprite = happy;
		rightRenderer.sprite = happy;
		won = true;
	}

	internal void SetHurt() {
		leftRenderer.sprite = hurtLeft;
		rightRenderer.sprite = hurtRight;
		lost = true;
	}
}
