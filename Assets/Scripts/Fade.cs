using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

class Fade : MonoBehaviour {

	[SerializeField] Animator animator;

	internal static Fade self;

	void Reset() {
		animator = GetComponent<Animator>();
	}

	void OnEnable() {
		self = this;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R) && !Emotion.self.won && !Emotion.self.lost) {
			StartCoroutine(LoadSceneCoru(SceneManager.GetActiveScene().buildIndex));
		}
	}

	internal void FadeOut() {
		animator.SetTrigger("FadeOut");
	}

	void NextScene() {
		int index = SceneManager.GetActiveScene().buildIndex + 1;
		if (index == SceneManager.sceneCountInBuildSettings) index = 1;
		StartCoroutine(LoadSceneCoru(index));
	}

	IEnumerator LoadSceneCoru(int scene) {
		AsyncOperation load = SceneManager.LoadSceneAsync(scene);
		while (!load.isDone) {
			yield return null;
		}
	}
}
