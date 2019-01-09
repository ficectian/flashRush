using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour {
	public string nextScene;
	private StageManage stageManage;
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		stageManage = GameObject.FindGameObjectWithTag("StageManage").GetComponent<StageManage>();
	}

	// Update is called once per frame
	void Update () {

	}

	// 碰撞开始
	private void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "Player"){
			stageManage.moveStageTo(nextScene);
		}
		
	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {

	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {
	}
}