﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		// this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
