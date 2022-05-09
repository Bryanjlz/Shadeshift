using UnityEngine;
using UnityEngine.SceneManagement;

class Goal : MonoBehaviour {

	[SerializeField] AnimationCurve curve;
	[SerializeField] float fadeDuration = 1f;
	float? startTime = null;
	bool done;

	void OnTriggerEnter2D(Collider2D collider) {
		if (startTime != null) return;
		if (!collider.CompareTag("Player")) return;
		print("level complete");
		Sound.self.PlayStar();
		Emotion.self.SetHappy();
		startTime = Time.time;
		LevelTracker.self.completion[SceneManager.GetActiveScene().buildIndex - 2] = true;
		LevelTracker.self.completeCount++;
	}

	void LateUpdate() {
		if (startTime == null) return;
		float elapsedTime = Time.time - (float)startTime;
		if (elapsedTime < fadeDuration) {
			float x = curve.Evaluate(elapsedTime / fadeDuration);
			transform.parent.localScale = new Vector3(x, x, x);
		} else if (!done) {
			Fade.self.FadeOut();
			done = true;
		}
	}
}
