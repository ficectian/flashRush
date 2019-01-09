using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  计时器
public struct Clock {
	public float elapsedt;
	public float outTimer;

	public void init (float outTime) {
		elapsedt = 0;
		outTimer = outTime;
	}
	public bool isTime () {
		if (elapsedt > outTimer) {
			return true;
		} else {
			return false;
		}

	}
	//时间流逝
	public void timeFlies () {
		elapsedt += Time.unscaledDeltaTime;
	}
}