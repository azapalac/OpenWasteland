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

	
	private const int DEFAULT = 0, MOVE = 1, SELECT = 2, ATTACK = 3, HARVEST = 4;
	public Image mouseCursor;
	private int mouseCursorState;
	public Vector3 mousePosition;
	public Sprite defaultCursor, moveCursor, attackCursor, selectCursor, harvestCursor;

	// Use this for initialization
	void Awake () {
		mouseCursorState = DEFAULT;
		mousePosition = new Vector3(0, 0, 0);
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
		mousePosition.x = Input.mousePosition.x;
		mousePosition.y = Input.mousePosition.y;
		mousePosition.z = 0;

		mouseCursor.rectTransform.position  = mousePosition;

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
