using UnityEngine;

class FlashlightToggler : MonoBehaviour {

	[SerializeField] GameObject target;
	bool justChanged = false;

	void Update() {
		if (Emotion.self.won || Emotion.self.lost) return;
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
		// if (Input.GetButtonDown("Fire1")) {
			Sound.self.PlayFlashOn();
			if (!justChanged && !target.activeSelf) {
				target.SetActive(true);
				justChanged = true;
			} else {
				justChanged = false;
			}
		} else if (Input.GetKeyUp(KeyCode.Mouse0)) {
		// } else if (Input.GetButtonUp("Fire1")) {
			Sound.self.PlayFlashOff();
			if (!justChanged && target.activeSelf) {
				target.SetActive(false);
			}
		}
	}
}
