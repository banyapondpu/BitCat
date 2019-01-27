﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFloor : MonoBehaviour
{
    public float speed = 0.04f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2 (Time.time * speed, 0);
		GetComponent<Renderer>().material.mainTextureOffset = offset;﻿
	}
}
