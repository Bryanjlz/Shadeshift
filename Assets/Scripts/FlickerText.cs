using UnityEngine;
using UnityEngine.UI;

class FlickerText : MonoBehaviour {

	[SerializeField] float minPixels = 40f;
	[SerializeField] float flickerRange = 20f;
	[SerializeField] float flickerRate = 5f;
	[SerializeField] int interval = 13;
	[SerializeField] CanvasScaler scaler;
	int frame = 0;

	void Reset() {
		scaler = GetComponent<CanvasScaler>();
	}

	void LateUpdate() {
		frame = (frame + 1) % interval;
		if (frame != 0) return;
		scaler.dynamicPixelsPerUnit = minPixels + Mathf.PingPong(Time.time * flickerRate, flickerRange);
	}
}
