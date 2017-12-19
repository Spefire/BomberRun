using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RayGame : NetworkBehaviour {

	public float speed = 10.0f;
	private int orientation;
	private float startSize;
	private float currentSize;
	private bool canExpand;
	private BombGame bg;

	//---------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------

	void Start() {
		startSize = this.transform.localScale.x;
		currentSize = 0.0f;
	}

	[ServerCallback]
	void Update () {
		if (canExpand) {
			if (currentSize < (bg.powerLevel - startSize + 0.5f)) {
				currentSize += speed * Time.deltaTime;
				this.transform.localScale += new Vector3 (speed * Time.deltaTime, 0, 0);
				switch (orientation) {
				case 1:
					this.transform.position += new Vector3 (-speed * Time.deltaTime / 2, 0, 0);
					break;
				case 2:
					this.transform.position += new Vector3 (0, 0, speed * Time.deltaTime / 2);
					break;
				case 3:
					this.transform.position += new Vector3 (speed * Time.deltaTime / 2, 0, 0);
					break;
				case 4:
					this.transform.position += new Vector3 (0, 0, -speed * Time.deltaTime / 2);
					break;
				}
			} else {
				NetworkServer.Destroy (this.gameObject);
			}
		}
	}

	public void SetBombGame(BombGame bg, int orientation) {
		this.bg = bg;
		this.canExpand = true;
		this.orientation = orientation;
	}

	void OnTriggerEnter(Collider obj) {
		if (obj.gameObject.tag.Equals ("Player")) {
			bg.GetPlayerGame().lifes--;
		}
		if (obj.gameObject.tag.Equals ("Box")) {
			NetworkServer.Destroy (obj.gameObject);
			NetworkServer.Destroy (this.gameObject);
		}
	}
	
}
