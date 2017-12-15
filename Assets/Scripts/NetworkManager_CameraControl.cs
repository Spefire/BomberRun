using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_CameraControl : NetworkLobbyManager {

	[Header("Scene Camera Properties")]
	public Transform sceneCamera;
	public Vector3 cameraPosition = Vector3.zero;
	public float cameraRotationRadius = 24f;
	public float cameraRotationSpeed = 3f;
	public bool canRotate = true;
	private float rotation;

	public override void OnStartClient(NetworkClient client) {
		canRotate = false;
	}

	public override void OnStartHost() {
		canRotate = false;
	}

	public override void OnStopClient() {
		canRotate = true;
	}

	public override void OnStopHost() {
		canRotate = true;
	}

	void Update () {
		if (!canRotate) {
			return;
		}
		CheckGround ();
		rotation += cameraRotationSpeed * Time.deltaTime;
		if (rotation >= 360f) {
			rotation -= 360f;
		}
		sceneCamera.position = cameraPosition;
		sceneCamera.rotation = Quaternion.Euler(0f, rotation, 0f);
		sceneCamera.Translate (0f, cameraRotationRadius, -cameraRotationRadius);
		sceneCamera.LookAt (cameraPosition);
	}

	private void CheckGround() {
		if (cameraPosition == Vector3.zero) {
			GameObject ground = GameObject.FindWithTag ("Ground");
			if (ground != null) {
				cameraPosition = ground.transform.position;
			}
		}
	}
}
