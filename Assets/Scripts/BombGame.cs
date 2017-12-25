﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BombGame : NetworkBehaviour {

	public GameObject rayPrefab;
	public float bombLifetime = 5.0f;
	public int powerLevel = 2;
	private float age;
	private int id;


	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start () {
		age = 0.0f;
		UpdateLooseBomb ();
	}

	[ServerCallback]
	void Update () {
		age += Time.deltaTime;
		if (age > bombLifetime) {
			UpdateGainBomb ();
			//1
			GameObject g = (GameObject) Instantiate (rayPrefab, this.transform.position, this.transform.rotation);
			g.GetComponent<RayGame> ().SetBombGame (this, 1);
			NetworkServer.Spawn (g);
			//2
			this.transform.Rotate (new Vector3 (0, 90, 0));
			g = (GameObject) Instantiate (rayPrefab, this.transform.position, this.transform.rotation);
			g.GetComponent<RayGame> ().SetBombGame (this, 2);
			NetworkServer.Spawn (g);
			//3
			this.transform.Rotate (new Vector3 (0, 90, 0));
			g = (GameObject) Instantiate (rayPrefab, this.transform.position, this.transform.rotation);
			g.GetComponent<RayGame> ().SetBombGame (this, 3);
			NetworkServer.Spawn (g);
			//4
			this.transform.Rotate (new Vector3 (0, 90, 0));
			g = (GameObject) Instantiate (rayPrefab, this.transform.position, this.transform.rotation);
			g.GetComponent<RayGame> ().SetBombGame (this, 4);
			NetworkServer.Spawn (g);
			//
			NetworkServer.Destroy (gameObject);
		}
	}

	public void UpdateLooseBomb(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			PlayerGame pg = player.GetComponent<PlayerGame> ();
			//Debug.LogError ("Player Game ID : " + pg.id + "/" + id);
			if (pg.id == id) {
				pg.LooseBomb ();
			}
		}
	}

	public void UpdateGainBomb(){
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in players) {
			PlayerGame pg = player.GetComponent<PlayerGame> ();
			//Debug.LogError ("Player Game ID : " + pg.id + "/" + id);
			if (pg.id == id) {
				pg.GainBomb ();
			}
		}
	}

	public void SetIDPlayerGame(int id) {
		this.id = id;
	}
}
