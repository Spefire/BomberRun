﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController: NetworkBehaviour {

	[Header("Camera variables")]
	public float sensitivityX = 1.5f;
	public float sensitivityY = 1.0f;
	public float smoothing = 2.0f;
	public float cameraHeight = 0.5f;
	public float cameraDistance = 0.1f;
	private Vector2 mouseLook;
	private Vector2 smoothV;
	private Transform cam;
	private Vector3 camOffset;

	[Header("Mouvment variables")]
	public float speed = 2.0f;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start() {
		if (!isLocalPlayer)	{
			Destroy (this);
			return;
		}
		cam = Camera.main.transform;
		camOffset = new Vector3 (0, cameraHeight, cameraDistance);
	}

	void Update () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		MoveBody ();
		MoveCamera ();
	}

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	private void MoveBody() {
		float translation = Input.GetAxis ("Vertical") * speed;
		float straffe = Input.GetAxis ("Horizontal") * speed;
		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;
		transform.Translate (straffe, 0, translation);
	}

	private void MoveCamera() {
		Vector2 md = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));

		md = Vector2.Scale(md, new Vector2(sensitivityX * smoothing, sensitivityY * smoothing));
		smoothV.x = Mathf.Lerp (smoothV.x, md.x, 1f / smoothing);
		smoothV.y = Mathf.Lerp (smoothV.y, md.y, 1f / smoothing);
		mouseLook += smoothV;

		if (mouseLook.y < -45) {
			mouseLook.y = -45;
		} else if (mouseLook.y > 45) {
			mouseLook.y = 45;
		}
		cam.position = transform.position + camOffset;
		cam.localRotation = Quaternion.AngleAxis (-mouseLook.y, cam.transform.right) * Quaternion.AngleAxis (mouseLook.x, Vector3.up);
		transform.localRotation = Quaternion.AngleAxis (mouseLook.x, Vector3.up);
	}
} 