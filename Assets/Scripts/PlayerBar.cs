using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBar : MonoBehaviour {
	public Vector2 initPosition;
	public float userBreakTime = 8.0f;
	public float deadAnimeTime = 1.5f;
	public float deadTime = 0.4f;
	private float toDieTime;
	private Clock breakTime;
	private Clock dieTime;
	private Clock toDeadTime;
	private bool isBeDeaing;
	private Animator animator;
	public int usedPower;
	private Player player;
	// Use this for initialization
	void Start () {
		transform.position = initPosition;
		init (initPosition, usedPower);
	}

	// Update is called once per frame
	void Update () {
		if (!isBeDeaing) {
			breakTime.timeFlies ();
			dieTime.timeFlies ();
			if (dieTime.isTime ()) {
				animator.SetBool ("isWillDie", true);
			}
			if (breakTime.isTime ()) {
				toDie ();
			}
		} else {
			toDeadTime.timeFlies ();
			if (toDeadTime.isTime ()) {
				breakSelf ();
			}
		}

	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			other.GetComponent<Player> ().highJump ();
			toDie ();
		}
	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {

	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {

	}

	public void init (Vector2 initPt, int power) {
		initPosition = initPt;
		breakTime.init (userBreakTime);
		usedPower = power;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		animator = GetComponent<Animator> ();
		toDieTime = userBreakTime - deadAnimeTime;
		dieTime.init (toDieTime);
		toDeadTime.init (deadTime);

		// animator.SetBool ("isToDie", false);
	}

	private void toDie () {
		isBeDeaing = true;
		animator.SetBool ("isToDie", true);
	}
	private void breakSelf () {
		if (player != null & player.state != Player.State.dead) {
			player.recoveryRushPower (usedPower);
		}
		Destroy (this.gameObject);
	}

	public void changeFace (bool isToRight) {
		if (!isToRight) {
			transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
		} else {
			transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
		}
	}
}