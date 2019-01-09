using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMachine : MonoBehaviour {
	float timeSpeed;

	Clock stopTime;

	bool isInited = false;
	// Use this for initialization
	void Start () {
		stopTime.elapsedt = 0.0f;

	}

	// Update is called once per frame
	void Update () {

		if (isInited) {
			stopTime.timeFlies ();
			if (stopTime.isTime ()) {
				Time.timeScale = 1.0f;
				Destroy (this.gameObject);
			}
		} else {
			Debug.Log ("time flase");
		}
	}

	public void timeStop (float stopSecond) {
		timeSpeed = 0.1f;
		stopTime.outTimer = stopSecond;
		isInited = true;
		Time.timeScale = timeSpeed;
	}
}