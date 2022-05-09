using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	[Header("Set these in editor")]
	[SerializeField] int COLS, ROWS;
	[SerializeField] Transform levelGridLayout;
	[SerializeField] GameObject levelButton;
	[SerializeField] Text titleText;
	[SerializeField] TextController textController;
	[SerializeField] FlickerText flickerText;
	[SerializeField] GameObject player;
	[SerializeField] Text bigTitleText;

	//public List<Scene> levels;
	public int levelIndexStart, levelIndexEnd;
	List<string> levelNames;

	//Mock values
	[Header("Debug, do not edit.")]
	public int pageNumber = 0;
	public int levelCount;
	public float xStart = -7;
	public float yStart = 1;
	public float step = 3.5f;



	// Start is called before the first frame update
	void Start() {
		levelNames = new List<string>();
		//Get levels
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
			string currentScene = SceneUtility.GetScenePathByBuildIndex(i);
			currentScene = currentScene.Substring(currentScene.LastIndexOf("/") + 1, currentScene.LastIndexOf(".") - currentScene.LastIndexOf("/") - 1);
			if (!currentScene.Equals("Level Select") && !currentScene.Equals("Title") && !currentScene.Equals("Thanks")) {
				levelNames.Add(currentScene);
			}
		}
		levelCount = levelNames.Count;

		LoadLevels();
	}

	void LoadLevels() {
		foreach (Transform child in levelGridLayout) {
			Destroy(child.gameObject);
		}
		for (int i = 0; i < COLS * ROWS; i++) {
			if (i < levelCount) {
				GameObject g = Instantiate(levelButton);
				LevelSelectButton button = g.transform.GetChild(2).gameObject.GetComponent<LevelSelectButton>();
				button.manager = this;
				if (i < levelNames.Count) {
					button.levelName = levelNames[i];
				} else {
					button.levelName = "You didn't set enough level names. No seriously, you have to fix this.";
				}
				g.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = string.Format("{0}", i + 1);
				g.transform.position = new Vector3(xStart + step * ((i % COLS) - 1), yStart - step * (i / COLS));
				g.transform.SetParent(levelGridLayout);
				button.anchor = g.transform.position;
				if (LevelTracker.self.completion[i]) {
					GameObject star = g.transform.GetChild(4).gameObject;
					star.SetActive(true);
					star.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));
                }
			}
		}
		if (LevelTracker.self.completeCount >= levelNames.Count) {
			player.SetActive(true);
			Emotion.self.SetHappy();
			bigTitleText.text = "THANKS FOR PLAYING!\n\n";
        }

	}
	public void SetTitle(string text) {
		textController.ResetFade();
		titleText.text = text;
		textController.enabled = true;
		textController.state = State.FADE;
	}
}
