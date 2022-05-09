using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyButton : MonoBehaviour
{
	[SerializeField] ButtonType buttonType;
	[SerializeField] public string sceneName;
	public bool hovered = false;
	[SerializeField ] Vector3 hoveredScale = new Vector3 (1.2f,1.2f,1f);
	private Vector3 normalScale = new Vector3 (1f, 1f,1f);

    private void Update() {
        if (hovered) {
			transform.parent.localScale = hoveredScale;
		} else {
			transform.parent.localScale = normalScale;
		}
    }

    private void OnMouseOver() {
		hovered = true;
		if (Input.GetMouseButtonUp(0)) {
			switch (buttonType) {
				case ButtonType.RESUME:
					PauseController pauseController = transform.parent.parent.parent.gameObject.GetComponent<PauseController>();
					pauseController.Unpause();
					if (SceneManager.GetActiveScene().name != "Umbra") pauseController.toggler.enabled = true;
					break;
				case ButtonType.SCENE_CHANGE:
					SceneManager.LoadScene(sceneName);
					break;
			}
		}
	}
    private void OnMouseExit() {
		hovered = false;
    }

    private enum ButtonType {
		RESUME,
		SCENE_CHANGE
	}
}
