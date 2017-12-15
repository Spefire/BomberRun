using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager_CameraControl : NetworkLobbyManager {

	[Header("Scene Camera Properties")]
	public Transform sceneCamera;
	public float cameraRotationRadius = 24f;
	public float cameraRotationSpeed = 3f;
	public bool canRotate = true;
	private float cameraRotation;
	private Vector3 cameraPosition;
	private GenerateMap genMap;

	void Start () {
		genMap = GetComponent<GenerateMap> ();
		cameraPosition = genMap.CreateMap ("map01.csv");
		DontDestroyOnLoad (sceneCamera);
	}

	public override void OnLobbyStartClient(NetworkClient client) {
		canRotate = false;
		genMap.CreateStructure ();
	}

	public override void OnLobbyStartHost() {
		canRotate = false;
		genMap.CreateStructure ();
	}

	public override void OnLobbyStopClient() {
		canRotate = true;
	}

	public override void OnLobbyStopHost() {
		canRotate = true;
	}

	void Update () {
		if (!canRotate) {
			return;
		}
		cameraRotation += cameraRotationSpeed * Time.deltaTime;
		if (cameraRotation >= 360f) {
			cameraRotation -= 360f;
		}
		sceneCamera.position = cameraPosition;
		sceneCamera.rotation = Quaternion.Euler(0f, cameraRotation, 0f);
		sceneCamera.Translate (0f, cameraRotationRadius, -cameraRotationRadius);
		sceneCamera.LookAt (cameraPosition);
	}
}
