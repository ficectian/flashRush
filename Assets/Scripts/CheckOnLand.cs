using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnLand : MonoBehaviour {
	public bool isOnland;
	private Player player;

	// Use this for initialization
	void Start () {
		//isOnland = true;
		player = transform.root.gameObject.GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update () {

	}

	// 碰撞开始
	private void OnTriggerEnter2D (Collider2D other) {
		if (player.state != Player.State.die && player.state != Player.State.dead) {	//玩家死亡时不进行处理
			isOnland = true;
			player.landing ();
			if (other.tag == "Land") {
				player.toBeSonWith (other.transform);
			}
		}

	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {
		isOnland = false;
		if (other.tag == "Land") {
			player.toBeFree ();
		}
	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {
		isOnland = true;
	}

}