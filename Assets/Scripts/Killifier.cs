using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

class Killifier : MonoBehaviour {

	[SerializeField] bool lightOnly;

	void OnTriggerEnter2D(Collider2D collider) {
		if (Emotion.self.won || Emotion.self.lost) return;
		if (lightOnly) {
			Player player = collider.GetComponent<Player>();
			if (player.Lit) return;
		}
		Emotion.self.SetHurt();
		Sound.self.PlayHurt();
		StartCoroutine(Crunch(0.75f));
	}

	IEnumerator Crunch(float delay) {
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime(delay);
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
