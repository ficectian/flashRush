using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
	public TimeMachine timeMachine;
	public float speed = 10.0f;

	public float attackStopTime;
	public float distance = 10000.0f;
	public int usedPower;
	public Vector2 initPosition;
	private Player player;

	// Use this for initialization
	public void init (Vector2 initPt, float bulletSpeed, float stopTime, float flyDistance, int power) {
		initPosition = initPt;
		speed = bulletSpeed;
		attackStopTime = stopTime;
		distance = flyDistance;
		usedPower = power;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

	}

	void Start () {
		init (initPosition, speed, attackStopTime, distance, usedPower);
		transform.position = initPosition;

	}

	// Update is called once per frame
	void Update () {
		transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
		float flyDistance = Mathf.Abs (transform.position.x - initPosition.x);

		if (flyDistance >= distance) {
			breakSelf ();
		}
	}

	//在摄像头外时
	void OnBecameInvisible () {
		breakSelf ();
	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Enemy") {
			if (this.transform.position.x > other.transform.position.x) {
				// collider.GetComponent<Enemy> ().underAttack (true, 1, attackPower, 1f, 5.0f);
			} else {
				// collider.GetComponent<Enemy> ().underAttack (false, 1, attackPower, 1f, 5.0f);
			}
			TimeMachine stopTime = Instantiate (timeMachine);
			stopTime.timeStop (attackStopTime);

			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraManage> ().camaraShockFor (attackStopTime);

			// TimeMachine stopTime = new TimeMachine ();
			// Instantiate (stopTime);
		}
		breakSelf ();
	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {

	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {
		breakSelf ();

	}

	public void breakSelf () {
		if (player.isAlive()) {
			player.recoveryRushPower (usedPower);
		}
		Destroy (this.gameObject);
	}
}