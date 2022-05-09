using UnityEngine;

class Enabler : MonoBehaviour {
	[SerializeField] GameObject target;

	void OnEnable() {
		target.SetActive(true);
	}
}
