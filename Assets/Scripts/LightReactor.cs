using UnityEngine;
using System.Collections.Generic;

abstract class LightReactor : MonoBehaviour {

	HashSet<Flashlight> lighters = new HashSet<Flashlight>();
	internal bool Lit => lighters.Count > 0;

	// these should be sealed
	internal void ParseLightEnter(Flashlight light) {
		// if (gameObject.name == "DarkCrateDebug") {
		// 	print("enter");
		// }
		if (!Lit) OnLightEnter();
		lighters.Add(light);
	}

	internal void ParseLightExit(Flashlight light) {
		// if (gameObject.name == "DarkCrateDebug") {
		// 	print("exit");
		// }
		lighters.Remove(light);
		if (!Lit) OnLightExit();
	}

	abstract internal void OnLightEnter();
	abstract internal void OnLightExit();
}
