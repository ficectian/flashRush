using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour {

	public enum State {
		standby, //待机
		run, //奔跑（移动）
		slowDown, //贴墙缓慢下落
		dead //死亡
	}
	private float maxScale = 1f;
	public float minScale = 0.45f;
	private Animator animator;

	private Vector3 initScale;
	private State state;

	private GameObject player;
	// Use this for initialization
	void Start () {
		player = transform.root.gameObject;
		initScale = transform.localScale;
		animator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		setScale ();

	}

	private void setScale () {
		float nowRushPower = player.GetComponent<Player> ().rushPower.nowValue;
		float maxRushPower = player.GetComponent<Player> ().rushPower.maxValue;

		float nowScale = (nowRushPower / maxRushPower) * (maxScale - minScale) + minScale;

		transform.localScale = nowScale * initScale;

	}

	//======================================================
	//	变更状态函数
	//======================================================
	public void moveStateTo (State movedState) {
		switch (movedState) {
			case State.slowDown: //变更玩家状态到 缓慢下落

				break;
			case State.standby:

				break;
			case State.dead:
				animator.SetBool("isDead",true);
				break;
		}

		state = movedState; //玩家的状态 = 要更改的状态
	}
}