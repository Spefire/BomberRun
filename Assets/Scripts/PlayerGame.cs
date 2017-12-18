﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerGame : NetworkBehaviour {

	[Header("Stats variables")]
	public int lifes = 3;

	[Header("Power variables")]
	public GameObject bombPrefab;
	public GameObject zonePrefab;
	public int numberBombs = 2;
	public float distanceAvaible = 5.0f;
	private GameObject zone;
	private bool canPose;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start() {
		canPose = false;
		zone = (GameObject) Instantiate (zonePrefab, new Vector3(0, -100.0f, 0), this.transform.rotation);
	}

	void Update () {
		if (!isLocalPlayer)	{
			return;
		}
		DisplayZone ();
		if (canPose && Input.GetMouseButtonDown (0)) {
			CmdBomb ();
		}
	}

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	private void DisplayZone() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider != null && numberBombs >= 1) {
				float x = Mathf.Round (hit.point.x);
				float y = Mathf.Floor (hit.point.y);
				float z = Mathf.Round (hit.point.z);
				Vector3 target = new Vector3 (x, y, z);
				float dist = Vector3.Distance (this.transform.position, target);
				if (-0.5f < y && y < 0.5f && dist <= distanceAvaible && MapGeneration.currentMap.IsEmpty((int) x, (int) z)) {
					canPose = true;
					zone.transform.position = target;
				} else {
					canPose = false;
					zone.transform.position = new Vector3 (0, -100.0f, 0);
				}
			} else {
				canPose = false;
				zone.transform.position = new Vector3 (0, -100.0f, 0);
			}
		}
	}

	[Command]
	void CmdBomb() {
		var bomb = (GameObject) Instantiate (bombPrefab, new Vector3(zone.transform.position.x, 0.5f, zone.transform.position.z), this.transform.rotation);
		NetworkServer.Spawn (bomb);
	}
} 