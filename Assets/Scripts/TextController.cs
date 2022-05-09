using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
	[SerializeField]
	Text text;
	[SerializeField]
	float inDelay;
	[SerializeField]
	float stayDelay;
	[SerializeField]
	bool willFadeOut;
	[SerializeField]
	Vector2 activation;
	[SerializeField]
	float fadeTime;
	[SerializeField]
	Transform player;

	public State state = 0;
	FlickerText ft;
	public CanvasScaler cs;
	float vel = 0f;

	private void Start() {
		cs = GetComponent<CanvasScaler>();
		ft = GetComponent<FlickerText>();
		cs.dynamicPixelsPerUnit = 0;
	}

	private void Update() {
		switch (state) {
			case State.FADE:
				FadeIn();
				break;
			case State.PAUSE:
				Pause();
				break;
			case State.FADEOUT:
				FadeOut();
				break;
			case State.NONE:
				if (inDelay != -1) {
					if (Time.timeSinceLevelLoad > inDelay) {
						state = State.FADE;
					}
				} else if (!activation.Equals(new Vector2(0, 0)) && player.position.x > activation.x) {
					state = State.FADE;
				}
				break;
		}
	}

	private void FadeIn() {
		cs.dynamicPixelsPerUnit = Mathf.SmoothDamp(cs.dynamicPixelsPerUnit, 40f, ref vel, fadeTime);
		if (40f - cs.dynamicPixelsPerUnit < 7f) {
			if (willFadeOut) {
				state = State.PAUSE;
				ft.enabled = true;
			} else {
				ft.enabled = true;
				enabled = false;
            }
		}
	}

	private void Pause() {
		if (Time.timeSinceLevelLoad > inDelay + stayDelay) {
			ft.enabled = false;
			state = State.FADEOUT;
		}
	}

	private void FadeOut() {
		cs.dynamicPixelsPerUnit = Mathf.SmoothDamp(cs.dynamicPixelsPerUnit, 0f, ref vel, fadeTime);
		if (cs.dynamicPixelsPerUnit - 7f < 0f) {
			gameObject.SetActive(false);
		}
	}

	public void ResetFade() {
		enabled = true;
		ft.enabled = false;
		state = State.NONE;
		cs.dynamicPixelsPerUnit = 0;
	}
}

