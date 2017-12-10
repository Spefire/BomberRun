using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MouseLook : NetworkBehaviour {

	public float sensitivityX = 1.0f;
	public float sensitivityY = 1.0f;
	public float smoothing = 2.0f;
	private Vector2 mouseLook;
	private Vector2 smoothV;
	private GameObject character;

	void Start() {
		character = this.transform.parent.gameObject;
	}

	void Update() {
		if (!character.GetComponent<PlayerMove>().isLocalPlayer)	{
			return;
		}
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
		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
		character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up);
	}
}