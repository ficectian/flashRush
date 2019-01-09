using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManage : MonoBehaviour {
	Clock shockTime;

	class ShockSwitch {
		public bool left;
		public bool right;
		public bool up;
		public bool down;

		public ShockSwitch () {
			left = false;
			right = false;
			up = false;
			down = false;
		}

		public void reSet () {
			left = false;
			right = false;
			up = false;
			down = false;
		}
	}

	ShockSwitch shockSwitch = new ShockSwitch ();

	private bool isToShock;
	float shockWidth = 0.1f;
	public float moveSpeed = 6f;
	float shockSpeed = 8f;

	float moveLimit = 2f;
	private Camera camera;

	const float size1080P = 8.4375f;

	// public float moveSpeed = 10f;
	private GameObject player;
	Vector3 initPosition;

	Vector3 zeroPosition () {
		// return new Vector3 (player.transform.position.x, player.transform.position.y + 2, transform.position.z);
		return new Vector3 (0f, 0f, -10f);
	}

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		camera = GetComponent<Camera> ();
		isToShock = false;
		initPosition = transform.position;

		camera.orthographicSize = size1080P;
	}

	// Update is called once per frame
	void Update () {
		if (isToShock) {
			camaraShock ();
		}
		// jumpMove ();
		// operation ();
		// scaleController ();
	}

	public void camaraShockFor (float seconds) {
		shockTime.elapsedt = 0.0f;
		shockTime.outTimer = seconds;
		isToShock = true;
	}

	// 相对坐标移动
	void moveTo (Vector2 position) {
		Vector3 wantPosition = new Vector3 (zeroPosition ().x + position.x, zeroPosition ().y + position.y, zeroPosition ().x);
		Vector3 movePosition = transform.position;

		if (wantPosition.x > transform.position.x) {
			movePosition.x += moveSpeed * Time.deltaTime;
			if (movePosition.x > wantPosition.x) {
				movePosition.x = wantPosition.x;
			}
		} else if (wantPosition.x < transform.position.x) {
			movePosition.x -= moveSpeed * Time.deltaTime;
			if (movePosition.x < wantPosition.x) {
				movePosition.x = wantPosition.x;
			}
		}

		if (wantPosition.y > transform.position.y) {
			movePosition.y += moveSpeed * Time.deltaTime;
			if (movePosition.y > wantPosition.y) {
				movePosition.y = wantPosition.y;
			}
		} else if (wantPosition.y < transform.position.y) {
			movePosition.y -= moveSpeed * Time.deltaTime;
			if (movePosition.y < wantPosition.y) {
				movePosition.y = wantPosition.y;
			}
		}

		transform.position = movePosition;

	}

	void ScaleTo (float size) {
		const float scaleSpeed = 2f;
		if (size > camera.orthographicSize) {
			camera.orthographicSize += scaleSpeed * Time.deltaTime;
			if (camera.orthographicSize > size) {
				camera.orthographicSize = size;
			}
		} else if (size < camera.orthographicSize) {
			camera.orthographicSize -= scaleSpeed * Time.deltaTime;
			if (camera.orthographicSize < size) {
				camera.orthographicSize = size;
			}
		}
	}

	void moveUp () {

	}
	void moveToDown () {
		Vector3 movePosition = Vector3.zero;
		movePosition.y -= moveLimit;
		moveTo (movePosition);
	}

	void moveBack () {
		moveTo (Vector3.zero);
	}

	void jumpMove () {
		// if (!player.GetComponent<Player> ().isGrounded) {
		// 	moveToDown ();
		// } else {
		// 	if (transform.position != zeroPosition ()) {
		// 		moveBack ();
		// 	}
		// }
	}

	void operation () {
		Vector2 wantPosition = Vector2.zero;
		if (Input.GetAxisRaw ("Horizontal2") > 0) {
			wantPosition.x += moveLimit;
		} else if (Input.GetAxisRaw ("Horizontal2") < 0) {
			wantPosition.x -= moveLimit;
		}
		if (Input.GetAxisRaw ("Vertical2") > 0) {
			wantPosition.y -= moveLimit;
		} else if (Input.GetAxisRaw ("Vertical2") < 0) {
			wantPosition.y += moveLimit;
		}

		// if (wantPosition == Vector2.zero && transform.position == zeroPosition()) {
		// 	float cameraInput = Input.GetAxisRaw ("Vertical");
		// 	if (cameraInput > 0) {
		// 		wantPosition.y += moveLimit;
		// 	} else if (cameraInput < 0) {
		// 		wantPosition.y -= moveLimit;
		// 	}
		// }

		moveTo (wantPosition);
	}

	void scaleController () {
		// // if (player.GetComponent<Player> ().state == Player.State.attack)
		// if (player.GetComponent<Player> ().isInRun () || !player.GetComponent<Player> ().isGrounded) {
		// 	ScaleTo (size720P);
		// } else {
		// 	if (camera.orthographicSize != size480P) {
		// 		ScaleTo (size480P);
		// 	}
		// }
	}
	void camaraShock () {
		shockTime.timeFlies ();
		// int shockMod = (int) (shockTime.elapsedt * 10) % 10;

		// switch (shockMod) {
		// 	case 0:
		// 		if (!shockSwitch.left) {
		// 			transform.position -= new Vector3 (shockWidth, 0.0f, 0.0f);
		// 			shockSwitch.left = true;
		// 		}
		// 		if (shockSwitch.right) {
		// 			shockSwitch.right = false;
		// 		}
		// 		break;
		// 	case 1:
		// 		if (!shockSwitch.up) {
		// 			transform.position += new Vector3 (0.0f, shockWidth, 0.0f);
		// 			shockSwitch.up = true;
		// 		}
		// 		if (shockSwitch.down) {
		// 			shockSwitch.down = false;
		// 		}
		// 		break;
		// 	case 2:
		// 		if (!shockSwitch.right) {
		// 			transform.position += new Vector3 (shockWidth, 0.0f, 0.0f);
		// 			shockSwitch.right = true;
		// 		}
		// 		if (shockSwitch.left) {
		// 			shockSwitch.left = false;
		// 		}
		// 		break;
		// 	case 3:
		// 		if (!shockSwitch.down) {
		// 			transform.position -= new Vector3 (0.0f, shockWidth, 0.0f);
		// 			shockSwitch.down = true;
		// 		}
		// 		if (shockSwitch.up) {
		// 			shockSwitch.up = false;
		// 		}
		// 		break;

		// }
		// 左右震动
		if (!shockSwitch.right) {
			transform.position += new Vector3 (shockSpeed * Time.unscaledDeltaTime, 0.0f, 0.0f);
			if (transform.position.x > zeroPosition ().x + shockWidth) {
				shockSwitch.right = true;
				shockSwitch.left = false;
			}
		} else if (!shockSwitch.left) {
			transform.position -= new Vector3 (shockSpeed * Time.unscaledDeltaTime, 0.0f, 0.0f);
			if (transform.position.x < zeroPosition ().x - shockWidth) {
				shockSwitch.left = true;
				shockSwitch.right = false;
			}
		}

		// 上下震动
		if (!shockSwitch.up) {
			transform.position += new Vector3 (0f, shockSpeed * Time.unscaledDeltaTime, 0.0f);
			if (transform.position.y > zeroPosition ().y + shockWidth) {
				shockSwitch.up = true;
				shockSwitch.down = false;
			}
		} else if (!shockSwitch.down) {
			transform.position -= new Vector3 (0f, shockSpeed * Time.unscaledDeltaTime, 0.0f);
			if (transform.position.y < zeroPosition ().y - shockWidth) {
				shockSwitch.down = true;
				shockSwitch.up = false;
			}
		}

		if (shockTime.isTime ()) {
			transform.position = zeroPosition ();
			shockTime.elapsedt = 0.0f;
			shockSwitch.reSet ();
			isToShock = false;
		}

	}
}