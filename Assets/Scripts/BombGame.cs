using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombGame : NetworkBehaviour {

	public float bombLifetime = 5.0f;
	private bool isLive = true;
	private float age;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start () {
		age = 0.0f;
	}

	[ServerCallback]
	void Update () {
		age += Time.deltaTime;
		if (age > bombLifetime) {
			NetworkServer.Destroy (gameObject);
		}
	}
}
