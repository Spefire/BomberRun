using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombGame : NetworkBehaviour {

	public float bombLifetime = 5.0f;
	private bool isLive = true;
	private float age;
	private PlayerGame pg;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start () {
		age = 0.0f;
	}

	[ServerCallback]
	void Update () {
		age += Time.deltaTime;
		if (age > bombLifetime) {
			pg.numberBombs++;
			NetworkServer.Destroy (gameObject);
		}
	}

	public void SetPlayerGame(PlayerGame pg) {
		this.pg = pg;
	}
}
