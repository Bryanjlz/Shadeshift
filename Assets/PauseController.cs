using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

	[SerializeField] internal FlashlightToggler toggler;
	[SerializeField] Player player;
	[SerializeField] Camera cam;
	[SerializeField] GameObject pauseScreen;
	[SerializeField] MyButton restartButton;
	[SerializeField] List<TextController> texts;
	[SerializeField] float delay;
	public bool paused = false;
	public bool canPause = true;

	private float timeSincePause;

    private void Start() {
		pauseScreen.SetActive(false);
		restartButton.sceneName = SceneManager.GetActiveScene().name;
	}

    // Update is called once per frame
    void Update() {
		if (canPause && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))) {
			if (paused) {
				Unpause();
				if (SceneManager.GetActiveScene().name != "Umbra") toggler.enabled = true;
			} else {
				Pause();
			}
		}
		if (paused) {
			player.Brake();
			for (int i = 0; i < texts.Count; i++) {
				if (timeSincePause + i * delay < Time.timeSinceLevelLoad) {
					texts[i].state = State.FADE;
                }
            }
		}
	}

	private void LateUpdate() {
		transform.position = (Vector2)cam.transform.position;
	}

	public void Unpause() {
		player.stop = false;
		paused = false;
		pauseScreen.SetActive(false);
		foreach (TextController text in texts) {
			text.ResetFade();
        }
	}

	public void Pause() {
		if (SceneManager.GetActiveScene().name != "Umbra") toggler.enabled = false; // reenabled in pause button
		timeSincePause = Time.timeSinceLevelLoad;
		player.stop = true;
		paused = true;
		pauseScreen.SetActive(true);
	}


}
