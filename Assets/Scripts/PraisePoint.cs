using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PraisePoint : MonoBehaviour {
	public GameObject praiseParticle;

	private bool isPraised;
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		isPraised = false;
	}

	// Update is called once per frame
	void Update () {

	}

	// 碰撞开始
	private void OnTriggerEnter2D (Collider2D other) {
		if (!isPraised) {
			if (other.tag == "Player") {
				Vector3 particlePosition = new Vector3 (other.transform.position.x, other.transform.position.y, 5);
				praiseParticle.transform.position = particlePosition;
				Instantiate (praiseParticle);
				isPraised = true;
				Destroy(this);
			}

		}
	}

	// 碰撞结束
	private void OnTriggerExit2D (Collider2D other) {

	}

	// 碰撞持续中
	private void OnTriggerStay2D (Collider2D other) {

	}

}