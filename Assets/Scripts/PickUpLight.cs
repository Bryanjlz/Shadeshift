using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpLight : MonoBehaviour {
	bool pickedUp = false;
	bool zoom = false;
	[SerializeField] PauseController pause;
	[SerializeField] Bobber bob;
	[SerializeField] Camera cam;
	[SerializeField] Transform flashlight;
	[SerializeField] Transform myLight;
	[SerializeField] FlashlightRotator rotator;
	[SerializeField] FlashlightToggler toggler;
	[SerializeField] Player player;
	[SerializeField] TextController text;
	[SerializeField] TextController arrow;
	[SerializeField] float zoomTime;
	[SerializeField] float textDelay;

	Vector2 camPos = new Vector3(-32, 0);
	Vector2 panVel = Vector2.zero;
	Vector2 posVel = Vector2.zero;
	Vector2 posVel2 = Vector2.zero;
	Vector3 rotVel = Vector3.zero;

	float timeStartZoom;

	// float camOrth = 9;
	float zoomVel = 0;

	float timeDelayStart;

	// Update is called once per frame
	void Update() {
		if (pickedUp && zoom) {
			player.Brake();
			cam.gameObject.transform.position = Vector2.SmoothDamp(cam.gameObject.transform.position, player.gameObject.transform.position, ref panVel, zoomTime);
			cam.gameObject.transform.position += new Vector3(0, 0, -10);
			cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, 3, ref zoomVel, zoomTime);
			transform.localPosition = Vector2.SmoothDamp(transform.localPosition, Vector2.zero, ref posVel, zoomTime);
			myLight.parent.localPosition = Vector2.SmoothDamp(myLight.parent.localPosition, flashlight.localPosition, ref posVel2, zoomTime);

			Vector3 rot = Vector3.SmoothDamp(myLight.parent.localRotation.eulerAngles, Vector3.zero, ref rotVel, zoomTime);
			myLight.parent.localRotation = Quaternion.Euler(rot);
			if (Time.timeSinceLevelLoad - timeStartZoom > zoomTime + 3) {
				zoom = false;
				player.stop = false;
				myLight.gameObject.SetActive(false);
				transform.GetChild(0).gameObject.SetActive(false);
				flashlight.GetComponent<EyePivot>().enabled = true;
				flashlight.parent.gameObject.SetActive(true);
				rotator.enabled = true;
				toggler.enabled = true;
				Emotion.self.SetNeutral();
				zoomTime -= 0.5f;
				timeDelayStart = Time.timeSinceLevelLoad;
			}
		} else if (pickedUp) {
			cam.gameObject.transform.position = Vector2.SmoothDamp(cam.gameObject.transform.position, camPos, ref panVel, zoomTime);
			cam.gameObject.transform.position += new Vector3(0, 0, -10);
			cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, 9, ref zoomVel, zoomTime);
			if (Time.timeSinceLevelLoad - timeStartZoom > 2 * (zoomTime + 3) - 1.5) {
				cam.gameObject.transform.position = camPos;
				cam.gameObject.transform.position += new Vector3(0, 0, -10);
				cam.orthographicSize = 9;
				text.enabled = true;
				arrow.enabled = true;
				pause.canPause = true;
				gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		pause.canPause = false;
		pickedUp = true;
		zoom = true;
		player.stop = true;
		bob.enabled = false;
		Emotion.self.SetHappy();
		transform.SetParent(rotator.transform);
		flashlight.GetComponent<EyePivot>().enabled = false;
		rotator.enabled = false;
		timeStartZoom = Time.timeSinceLevelLoad;
	}
}
