using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManage : MonoBehaviour {
	Scene thisScene;
	private static int startNum = 0;
	// Scene nextScene;
	Player player;

	// Use this for initialization
	void Start () {
		// DontDestroyOnLoad (this);
		init ();
	}

	// Update is called once per frame
	void Update () {
		if (!player.isAlive ()) {
			reloadStage ();
		}
	}

	private void init () {
		thisScene = SceneManager.GetActiveScene ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		player.startNum = startNum;
		Screen.SetResolution (1920, 1080, true);
		// Debug.Log (startNum);
		// Debug.Log (player.startNum);
	}
	public void reloadStage () {
		// Scene scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (thisScene.name);
	}

	public void moveStageTo (string nextScene) {
		SceneManager.LoadScene (nextScene);
	}

	public void changeStNumTo (int changNum) {
		startNum = changNum;
	}
}