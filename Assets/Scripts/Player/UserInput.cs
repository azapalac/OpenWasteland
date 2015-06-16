using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour {
	private Player player;
	private Camera mainCamera;
	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<Player>();
		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player.isHuman){
			MoveCamera();
			RotateCamera();
		}
	}

	private void MoveCamera(){
		float xPos = Input.mousePosition.x;
		float yPos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0, 0, 0);

		//X-direction Camera movement
		if(xPos >= 0 && xPos < ResourceManager.ScrollWidth){
			movement.x -= ResourceManager.ScrollSpeed;
		}else if(xPos <= Screen.width && xPos > Screen.width - ResourceManager.ScrollWidth){
			movement.x += ResourceManager.ScrollSpeed;
		}

		//Z-direction Camera movement
		if(yPos >= 0 && yPos < ResourceManager.ScrollWidth){

			movement.z -= ResourceManager.ScrollSpeed;
		}else if(yPos <= Screen.height && yPos > Screen.height - ResourceManager.ScrollWidth){
			movement.z += ResourceManager.ScrollSpeed;
		}

		//Zooming
		movement = mainCamera.transform.TransformDirection(movement);
		movement.y = 0;
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

		//Calculate desired camera position based on recieved input
		Vector3 origin = mainCamera.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;


		//Set zoom limits
		if(destination.y > ResourceManager.MaxCameraHeight){
			destination.y = ResourceManager.MaxCameraHeight;
		}else if(destination.y < ResourceManager.MinCameraHeight){
			destination.y = ResourceManager.MinCameraHeight;
		}

		//If a change is detected, provide the necessary updates
		if(destination != origin){
			mainCamera.transform.position = Vector3.MoveTowards (origin, destination, Time.deltaTime*ResourceManager.ScrollSpeed);
		}
	}

	private void RotateCamera(){
		Vector3 origin = mainCamera.transform.eulerAngles;
		Vector3 destination = origin;

		if((Input.GetKey(KeyCode.LeftAlt)||Input.GetKey(KeyCode.RightAlt))&& Input.GetMouseButton(1)){
			destination.x -= Input.GetAxis("Mouse Y")*ResourceManager.RotateAmount;
			destination.y += Input.GetAxis("Mouse X")*ResourceManager.RotateAmount;
		}

		if(destination != origin){
			mainCamera.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime*ResourceManager.RotateSpeed);
		}
	}

}
