using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCrash : MonoBehaviour {
	// private Player player;
	// Use this for initialization
	void Start () {
		// player = transform.root.gameObject.GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// 碰撞开始
	private void OnTriggerEnter2D (Collider2D other) {
		// if (other.tag == "Land") {
		// 	if (player.state != Player.State.slowDown) {
		// 		player.moveStateTo (Player.State.slowDown);
		// 		Debug.Log ("从 CheckCrash 进入缓慢下落");
		// 	}
		// }

	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {
		// if (other.tag == "Land") {
		// 	if (player.state == Player.State.slowDown) {
		// 		player.exitSlowDown ();
		// 		Debug.Log ("从 CheckCrash 离开缓慢下落");
		// 	}
		// }
	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {
		// if (other.tag == "Land") {
		// 	if (player.state != Player.State.slowDown) {
		// 		player.moveStateTo (Player.State.slowDown);
		// 	}
		// }

	}
}