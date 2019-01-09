using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownCheck : MonoBehaviour {

	private Player player;
	// public bool isFacedRight;
	// Use this for initialization
	void Start () {
		player = transform.root.gameObject.GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// 碰撞开始
	private void OnTriggerEnter2D (Collider2D other) {
		playerToSlowDown (other);
	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "Land") {
			if (player.state == Player.State.slowDown || player.isGrounded) {
				player.exitSlowDown ();
				player.isWannaToExitSlowDown = false;
			}
		}
	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {
		// playerToSlowDown (other);
	}

	private void playerToSlowDown (Collider2D other) {
		if (player.state != Player.State.die && player.state != Player.State.dead) { //玩家死亡时不进行处理
			if (other.tag == "Land") {
				if (player.state != Player.State.slowDown && !player.isGrounded && !player.isWannaToExitSlowDown) {
					player.moveStateTo (Player.State.slowDown);
					// if (other.transform.position.x < player.transform.position.x) {
					// 	isFacedRight = true;
					// } else {
					// 	isFacedRight = false;
					// }
				}
			}
		}

	}
}