using UnityEngine;

class CrateSounder : MonoBehaviour {

	[SerializeField] float speedThreshold = 1f;
	[SerializeField] float fallThreshold = 0.5f;
	[SerializeField] Rigidbody2D body;
	[SerializeField] AudioSource source;
	[SerializeField] Collider2D groundCheck;
	ContactPoint2D[] groundContacts = new ContactPoint2D[1];
	ContactFilter2D groundFilter = new ContactFilter2D {
		// useLayerMask = true, // needs to be in OnEnable
		// layerMask = LayerMask.GetMask("Ground"),
		useNormalAngle = true,
		minNormalAngle = 30f,
		maxNormalAngle = 150f
	};
	bool grounded => groundCheck.GetContacts(groundFilter, groundContacts) >= 1;
	float lastY;
	bool prevGrounded = false;

	void OnEnable() {
		lastY = body.position.y;
	}

	void FixedUpdate() {
		// if (!grounded) source.mute = true;
		if (!prevGrounded && grounded && lastY - body.position.y >= fallThreshold) {
			Sound.self.PlayThump();
		}
		if (grounded && Mathf.Abs(body.velocity.x) > 0f) {
			source.mute = false;
			source.volume = Mathf.Clamp(Mathf.Abs(body.velocity.x / speedThreshold), 0f, speedThreshold);
		} else {
			source.mute = true;
		}
		prevGrounded = grounded;
		if (grounded) lastY = body.position.y;
	}
}
