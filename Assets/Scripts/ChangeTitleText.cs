using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeTitleText : MonoBehaviour {
	[SerializeField] Text text;
	[SerializeField] Camera cam;

	// Start is called before the first frame update
	void Start() {

		Scene scene = SceneManager.GetActiveScene();
		//text.text = string.Format("{0}", scene.buildIndex) + ". " + scene.name;
		text.text = scene.name;
	}

	//private void LateUpdate() {
	//	transform.position = cam.transform.position;
	//	transform.position += new Vector3(0, 0, 1f);
	//}
}
