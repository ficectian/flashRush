using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLand : MonoBehaviour {
	public enum MoveMode {
		horizontal,
		vertical
	}

	public float moveSpeed = 1.0f;
	public float moveWidth = 3.0f;
	public bool isMoveToMax = true;
	private float moveMin;
	private float moveMax;
	public MoveMode moveMode = MoveMode.horizontal;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 2.0f);

		switch (moveMode) {
			case MoveMode.horizontal:
				moveMin = transform.position.x - moveWidth;
				moveMax = transform.position.x + moveWidth;
				break;
			case MoveMode.vertical:
				moveMin = transform.position.y - moveWidth;
				moveMax = transform.position.y + moveWidth;
				break;
		}

	}

	// Update is called once per frame
	void Update () {
		switch (moveMode) {
			case MoveMode.horizontal:
				if (isMoveToMax) {
					float moveX = transform.position.x + moveSpeed * Time.deltaTime;
					if (moveX > moveMax) {
						isMoveToMax = false;
					} else {
						transform.position = new Vector3 (moveX, transform.position.y, transform.position.z);
					}
				} else {
					float moveX = transform.position.x - moveSpeed * Time.deltaTime;
					if (moveX < moveMin) {
						isMoveToMax = true;
					} else {
						transform.position = new Vector3 (moveX, transform.position.y, transform.position.z);
					}
				}
				break;

			case MoveMode.vertical:
				if (isMoveToMax) {
					float moveY = transform.position.y + moveSpeed * Time.deltaTime;
					if (moveY > moveMax) {
						isMoveToMax = false;
					} else {
						transform.position = new Vector3 (transform.position.x, moveY, transform.position.z);
					}
				} else {
					float moveY = transform.position.y - moveSpeed * Time.deltaTime;
					if (moveY < moveMin) {
						isMoveToMax = true;
					} else {
						transform.position = new Vector3 (transform.position.x, moveY, transform.position.z);
					}
				}
				break;
		}
	}

	// private void OnTriggerEnter2D (Collider2D other) {
	// 	if (other.tag == "LandCheck") {
	// 		Player player = other.transform.root.gameObject.GetComponent<Player> ();
	// 		player.gameObject.GetComponent<Player> ().toBeSonWith (transform);
	// 	}

	// }

	// // 碰撞结束
	// private void OnTriggerExit2D (Collider2D other) {
	// 	if (other.tag == "LandCheck") {
	// 		Player player = other.transform.root.gameObject.GetComponent<Player> ();
	// 		player.toBeSonWith (transform);
	// 	}
	// }

	// // 碰撞持续中
	// private void OnTriggerStay2D (Collider2D other) {
	// 	// if (other.tag == "Land") {
	// 	// 	if (player.state != Player.State.slowDown) {
	// 	// 		player.moveStateTo (Player.State.slowDown);
	// 	// 	}
	// 	// }

	// }
}