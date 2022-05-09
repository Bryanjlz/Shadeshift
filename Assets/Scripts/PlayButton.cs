using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class PlayButton : MonoBehaviour {
	public bool hovered = false;
	[SerializeField] Transform textParent;
	[SerializeField ] Vector3 hoveredScale = new Vector3 (1.2f,1.2f,1f);
	private Vector3 normalScale = new Vector3 (1f, 1f,1f);

    private void Update() {
        if (hovered) {
			textParent.localScale = hoveredScale;
		} else {
			textParent.localScale = normalScale;
		}
    }

    private void OnMouseOver() {
		hovered = true;
		if (Input.GetMouseButtonUp(0)) {
			SceneManager.LoadScene(1);
		}
	}
    private void OnMouseExit() {
		hovered = false;
    }
}
