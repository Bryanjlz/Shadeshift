using UnityEngine;

class Sound : MonoBehaviour {

	internal static Sound self;
	[SerializeField] AudioSource source;
	[SerializeField] AudioClip[] jumpClips;
	[SerializeField] AudioClip[] hurtClips;
	[SerializeField] AudioClip[] flashOnClips;
	[SerializeField] AudioClip[] flashOffClips;
	[SerializeField] AudioClip[] gateClips;
	[SerializeField] AudioClip[] thumpClips;
	[SerializeField] AudioClip[] starClips;

	void OnEnable() {
		// if (self != null) {
		// 	Destroy(gameObject);
		// 	return;
		// }
		// DontDestroyOnLoad(gameObject);
		self = this;
	}

	void Reset() {
		source = GetComponent<AudioSource>();
	}

	void PlayRandom(AudioClip[] clips) {
		source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
	}

	internal void PlayJump() => PlayRandom(jumpClips);
	internal void PlayHurt() => PlayRandom(hurtClips);
	internal void PlayFlashOn() => PlayRandom(flashOnClips);
	internal void PlayFlashOff() => PlayRandom(flashOffClips);
	internal void PlayGate() => PlayRandom(gateClips);
	internal void PlayThump() => PlayRandom(thumpClips);
	internal void PlayStar() => PlayRandom(starClips);
}
