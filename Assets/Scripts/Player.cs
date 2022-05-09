using UnityEngine;
using System.Collections;

class Player : MonoBehaviour {

	[Header("Tuning")]
	[SerializeField] float jumpHeight = 3f;
	[SerializeField] float runSpeed = 6f;
	[SerializeField] float terminalVelocity = 20f;

	[Header("Setup")]
	[SerializeField] Rigidbody2D body;
	[SerializeField] Collider2D groundCheck;
	[SerializeField] LayerMask groundMask;
	ContactPoint2D[] groundContacts = new ContactPoint2D[1];
	ContactFilter2D groundFilter = new ContactFilter2D {
		// useLayerMask = true, // needs to be in OnEnable
		// layerMask = LayerMask.GetMask("Ground"),
		useNormalAngle = true,
		minNormalAngle = 30f,
		maxNormalAngle = 150f
	};

	public bool stop = false;

	bool Grounded => groundCheck.IsTouchingLayers(groundMask) && groundCheck.GetContacts(groundFilter, groundContacts) >= 1;
	[SerializeField] GameObject flashlight;
	internal bool Lit => flashlight.activeSelf;

	float lastVelocity;
	float drag;
	bool jumpQueued;

	void Reset() {
		body = GetComponent<Rigidbody2D>();
		groundCheck = GetComponent<Collider2D>();
	}

	void OnEnable() {
		drag = 1f - runSpeed / body.mass * Time.fixedDeltaTime;
		// body.drag = runSpeed / body.mass;
	}

	void Update() {
		if (!stop) {
			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
				jumpQueued = true;
			} else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow)) {
				Unjump();
			}
		}
	}

	void FixedUpdate() {
		if (!stop) {
			if (Input.GetAxisRaw("Horizontal") == 0f) { // braking
				// Brake();
			} else {
				Run(Input.GetAxisRaw("Horizontal"));
			}
			body.velocity = new Vector2(Mathf.Clamp(body.velocity.x * drag, -terminalVelocity, terminalVelocity), Mathf.Clamp(body.velocity.y, -terminalVelocity, terminalVelocity));
			if (jumpQueued && Grounded && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))) {
				Jump();
				jumpQueued = false;
			}
			if (body.velocity.y <= 0f) body.gravityScale = 1f;
		}
	}

	public void Brake() {
		if (lastVelocity == 0f) {
			lastVelocity = body.velocity.x;
			return;
		}
		if (body.velocity.x < 0f && lastVelocity < 0f || body.velocity.x > 0f && lastVelocity > 0f) {
			body.velocity = new Vector2(0f, body.velocity.y);
		} else {
			Run(-body.velocity.x);
		}
		lastVelocity = body.velocity.x;
	}

	void Run(float heading) {
		body.AddForce(new Vector2(Mathf.Clamp(heading, -1f, 1f) * runSpeed * runSpeed, 0f));
	}

	void Jump() {
		Sound.self.PlayJump();
		body.velocity = new Vector2(body.velocity.x, Mathf.Sqrt(jumpHeight * -Physics2D.gravity.y)); // usually mult by 2 but we halve gravity on upwards vel
		body.gravityScale = 0.5f;
	}

	void Unjump() {
		if (body.velocity.y <= 0f) return;
		body.velocity = new Vector2(body.velocity.x, Mathf.Sqrt(body.velocity.y));
	}
}
