using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour {
	internal static LevelTracker self;

    [SerializeField] public bool[] completion;
    [SerializeField] public int completeCount;

	void OnEnable() {
        if (self != null) {
            Destroy(gameObject);
            return;
        }
        completion = new bool[SceneManager.sceneCountInBuildSettings - 2];
        DontDestroyOnLoad(gameObject);
        self = this;
	}

}
