using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour {

	[Header("values set by scripts")]
	public string levelName;
	public LevelSelect manager;
    public Vector3 anchor;

    private void Update() {
        Wobble();
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
			SceneManager.LoadScene(levelName);
        }
    }

    private void OnMouseEnter() {
        manager.SetTitle(levelName.ToUpperInvariant());
    }
    private void OnMouseExit() {
        manager.SetTitle("");
    }

    private void Wobble() {
        transform.position = anchor + new Vector3 (Mathf.PingPong(Time.time * 0.001f ,0.1f), 0f);
    }
}
