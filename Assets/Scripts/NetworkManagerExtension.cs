using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerExtension : NetworkManager {

	[Header("Canvas Properties")]
	public GameObject menuPrincipal;
	public GameObject menuCredits;
	private bool isOnCredits;
	[Header("Scene Camera Properties")]
	public Transform sceneCamera;
	public float cameraRotationRadius = 25f;
	public float cameraRotationSpeed = 5f;
	public bool canRotate = true;
	private float cameraRotation;
	private Vector3 cameraPosition;
	private MapGeneration genMap;

	void Start () {
		isOnCredits = false;
		genMap = GetComponent<MapGeneration> ();
		genMap.CreateMap ("map01.csv");
		genMap.CreateStructure ();
		cameraPosition = genMap.GetMapPosition ();
	}

	public override void OnStartHost() {
		canRotate = false;
		genMap.CreateBoxes ();
		menuPrincipal.SetActive (false);
		menuCredits.SetActive (false);
		isOnCredits = false;
	}

	public override void OnStartClient(NetworkClient client) {
		canRotate = false;
		menuPrincipal.SetActive (false);
		menuCredits.SetActive (false);
		isOnCredits = false;
	}

	public override void OnStopHost() {
		canRotate = true;
		menuPrincipal.SetActive (true);
	}

	public override void OnStopClient() {
		canRotate = true;
		menuPrincipal.SetActive (true);
	}

	void Update () {
		if (!canRotate) {
			return;
		}

		if (!isOnCredits && Input.GetKeyDown (KeyCode.Q)) {
			Application.Quit ();
		}
		if (!isOnCredits && Input.GetKeyDown (KeyCode.D)) {
			menuPrincipal.SetActive (false);
			menuCredits.SetActive (true);
			isOnCredits = true;
		} else if (isOnCredits && Input.GetKeyDown (KeyCode.D)) {
			menuCredits.SetActive (false);
			menuPrincipal.SetActive (true);
			isOnCredits = false;
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
