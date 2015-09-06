using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RTS;

public class HUD : MonoBehaviour {
	public Image ordersBar, resourceBar;

	//Change color based on allegiance to player
	public GameObject selectionBox;
	private SpriteRenderer selectionBoxRenderer;
	private Vector3 size;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private Player player;
	public Text nameField;
	
	public Image mouseCursor;
	public Sprite defaultCursor, moveCursor, attackCursor, selectCursor, harvestCursor;

	// Use this for initialization
	void Awake () {
		Cursor.visible = false;
		mouseCursor.color = Color.black;
		//mouseCursor.rectTransform.position = Vector3.zero;
		player = transform.root.GetComponent<Player>();
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBoxRenderer.color = Color.clear;
		//size = this.gameObject.GetComponent<Collider>().bounds.size;
		//Debug.Log ("Size: " + size);
		ResourceManager.StoreSelectionBoxItems(selectionBox);
	}
	
	// Update is called once per frame
	void Update () {
		HandleGUI();
	}

	void HandleGUI(){
		string selectionName = "";
		if(player.SelectedObject){
			selectionName  = player.SelectedObject.objectName;
		}
		
		if(!selectionName.Equals("")){
			nameField.text = selectionName;
		}

		DrawMouseCursor();
	}


	private void DrawMouseCursor(){
		mouseCursor.rectTransform.position  = Input.mousePosition;

		if(player.SelectedObject){


		}else{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast (ray, out hit)){
				if(hit.collider.gameObject.layer == ResourceManager.WorldObjectLayer){
					mouseCursor.sprite = selectCursor;
					mouseCursor.color = Color.green;
				}else{
					mouseCursor.sprite = defaultCursor;
					mouseCursor.color = Color.black;
				}
			}else{
				mouseCursor.sprite = defaultCursor;
				mouseCursor.color = Color.black;
			}

		}
	}

	void OnGUI(){

	}

	public bool MouseInBounds(){

		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
		bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;

		return insideWidth && insideHeight;
	}
}
