using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {
	[SerializeField]
	Transform player;

	public bool pan = false;
	Vector3 currentScreen;
	Vector3 velocity = Vector3.zero;
	Vector3 panTarget;
	public float smoothTime = 0.3f;
	public float xMax = 16f;
	public float yMax = 9f;

    private void Start() {
		currentScreen = transform.position;
    }

    // Update is called once per frame
    void Update() {
		if (pan) {
			transform.position = Vector3.SmoothDamp(transform.position, panTarget, ref velocity, smoothTime);
			if(Vector3.Distance(transform.position, panTarget) < 0.001f) {
				pan = false;
			}
		}

		Vector3 posDiff = player.position - currentScreen;
		if (posDiff.x > xMax) {
			SetPan(new Vector2(xMax, 0f));
		} else if (posDiff.x < -xMax) {
			SetPan(new Vector2(-xMax, 0f));
		} else if (posDiff.y > yMax) {
			SetPan(new Vector2(0f, yMax));
		} else if (posDiff.y < -yMax) {
			SetPan(new Vector2(0f, -yMax));
		}
	}

	private void SetPan (Vector3 delta) {
		pan = true;
		panTarget = currentScreen + 2 * delta;
		currentScreen = panTarget;
	}

}
