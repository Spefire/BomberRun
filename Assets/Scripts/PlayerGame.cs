﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerGame : NetworkBehaviour {

	[Header("Stats variables")]
	public Canvas interf;
	public Text lifeText;
	public Text bombText;
	public int nbLifes = 3;

	[Header("Power variables")]
	public GameObject bombPrefab;
	public GameObject zonePrefab;
	public int nbBombs = 2;
	public float distanceAvaible = 5.0f;
	private GameObject zone;
	private bool canPose;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start() {
		if (!isLocalPlayer)	{
			interf.gameObject.SetActive (false);
			return;
		}
		canPose = false;
		interf.worldCamera = Camera.main;
		zone = (GameObject) Instantiate (zonePrefab, new Vector3(0, -100.0f, 0), this.transform.rotation);
		DisplayCurrentBombs ();
		DisplayCurrentLifes ();
	}

	void Update () {
		if (!isLocalPlayer)	{
			return;
		}
		DisplayZone ();
		if (canPose && Input.GetMouseButtonDown (0)) {
			CmdBomb ();
			LooseBomb ();
		}
	}

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	private void DisplayZone() {
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider != null && nbBombs >= 1) {
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

	public void GainBomb() {
		nbBombs++;
		DisplayCurrentBombs ();
	}

	public void LooseBomb() {
		nbBombs--;
		DisplayCurrentBombs ();
	}

	private void DisplayCurrentBombs(){
		if (nbBombs > 1) {
			bombText.text = nbBombs+" Bombes en stock";
		} else if (nbBombs == 1) {
			bombText.text = nbBombs+" Bombe en stock";
		} else {
			bombText.text = "Aucune Bombe";
		}
	}

	public void LooseLife(){
		nbLifes--;
		DisplayCurrentLifes ();
	}

	private void DisplayCurrentLifes() {
		if (nbLifes > 1) {
			lifeText.text = nbLifes+" Vies restantes";
		} else if (nbLifes == 1) {
			lifeText.text = nbLifes+" Vie restante";
		} else {
			lifeText.text = "Vous etes mort...";
		}
	}

	[Command]
	void CmdBomb() {
		var bomb = (GameObject) Instantiate (bombPrefab, new Vector3(zone.transform.position.x, 0.5f, zone.transform.position.z), Quaternion.identity);
		bomb.GetComponent<BombGame> ().SetPlayerGame (this);
		NetworkServer.Spawn (bomb);
	}
} 