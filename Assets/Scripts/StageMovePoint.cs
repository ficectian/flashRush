using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMovePoint : MonoBehaviour {
	public string nextScene; //下一个切换的场景
	public int nowStartNum;
	public int nextStartNum;
	private StageManage stageManage; //场景控制器
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = false; //不显示
		stageManage = GameObject.FindGameObjectWithTag ("StageManage").GetComponent<StageManage> ();

	}

	// Update is called once per frame
	void Update () {

	}

	// 碰撞开始
	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			stageManage.changeStNumTo(nextStartNum);
			stageManage.moveStageTo (nextScene);
		}

	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {

	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) { }
}