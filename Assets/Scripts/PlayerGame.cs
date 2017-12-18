﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerGame: NetworkBehaviour {

	[Header("Stats variables")]
	public int lifes = 3;

	[Header("Power variables")]
	public GameObject bomb;
	public int numberBombs = 2;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start() {
		/*if (!isLocalPlayer)	{
			Destroy (this);
			return;
		}*/
	}

	void Update () {
		if (!isLocalPlayer)	{
			return;
		}
		if (numberBombs >= 1 && Input.GetMouseButtonDown (0)) {
			CmdBomb ();
		}
	}

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	[Command]
	void CmdBomb() {
		var b = (GameObject)Instantiate(bomb, this.transform.position, this.transform.rotation);
		NetworkServer.Spawn(b);
	}
} 