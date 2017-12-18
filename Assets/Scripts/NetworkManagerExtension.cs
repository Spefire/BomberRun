using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerExtension : NetworkManager {

	[Header("Canvas Properties")]
	public GameObject canvas;
	[Header("Scene Camera Properties")]
	public Transform sceneCamera;
	public float cameraRotationRadius = 25f;
	public float cameraRotationSpeed = 5f;
	public bool canRotate = true;
	private float cameraRotation;
	private Vector3 cameraPosition;
	private MapGeneration genMap;

	void Start () {
		genMap = GetComponent<MapGeneration> ();
		genMap.CreateMap ("map01.csv");
		genMap.CreateStructure ();
		cameraPosition = genMap.GetMapPosition ();
	}

	public override void OnStartHost() {
		canRotate = false;
		genMap.CreateBoxes ();
		canvas.SetActive (false);
	}

	public override void OnStartClient(NetworkClient client) {
		canRotate = false;
		canvas.SetActive (false);
	}

	public override void OnStopHost() {
		canRotate = true;
		canvas.SetActive (true);
	}

	public override void OnStopClient() {
		canRotate = true;
		canvas.SetActive (true);
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
