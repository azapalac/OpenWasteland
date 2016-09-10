using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RTS;

public class HUD : MonoBehaviour {
    public Image ordersBar;
    public GameObject multiSelect;
    private Image multiSelectImage;
    private const float renderWidth = 20.0f;
	//Change color based on allegiance to player
	public GameObject selectionBox;
    public GameObject radius;
	private SpriteRenderer selectionBoxRenderer;
	private Vector3 size;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private Player player;
	public Text nameField;
    private bool paused;
	public Image mouseCursor;
	public Sprite defaultCursor, moveCursor, attackCursor, selectCursor, harvestCursor;
    public GameObject imagePrefab;
	// Use this for initialization
	void Awake () {
		Cursor.visible = false;
		mouseCursor.color = Color.black;
		//mouseCursor.rectTransform.position = Vector3.zero;
		player = transform.root.GetComponent<Player>();
		selectionBoxRenderer = selectionBox.GetComponent<SpriteRenderer>();
		selectionBoxRenderer.color = Color.clear;

		ResourceManager.StoreSelectionBoxItems(selectionBox);
        ResourceManager.StoreRadiusItems(radius);

       
        multiSelectImage = multiSelect.GetComponent<Image>();
        //multiSelect.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            paused = true;
        }else if(Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            paused = false;
        }
        
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

    //WELCOME TO RAYCAST HELL
	private void DrawMouseCursor(){
		mouseCursor.rectTransform.position  = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //standard behavior
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.layer == ResourceManager.WorldObjectLayer)
            {
                //Should be red for enemies (At War), green for allies, and yellow for neutral
                mouseCursor.sprite = selectCursor;
                mouseCursor.color = Color.green;
            } else {
                mouseCursor.sprite = defaultCursor;
                mouseCursor.color = Color.black;
            }

        }
        else {
            mouseCursor.sprite = defaultCursor;
            mouseCursor.color = Color.black;
        }

        //If an object is selected, override cursor
        if (player.SelectedObject){

            if(Physics.Raycast(ray, out hit)){
                
                if (player.SelectedObject.CanDo("Move") && hit.collider.gameObject.name == "Ground")
                {
                    mouseCursor.sprite = moveCursor;
                    mouseCursor.color = Color.black;
                }

                //check if the object is harvestable
                if (hit.collider.gameObject.GetComponent<HarvestableObject>() != null)
                {
                    HarvestableObject h = hit.collider.gameObject.GetComponent<HarvestableObject>();
                    if (player.SelectedObject.CanDo("Harvest " + h.type))
                    {
                        mouseCursor.sprite = harvestCursor;
                        mouseCursor.color = Color.yellow;
                    }
                }
            }
		}
	}

    public void StartMultiSelect(Vector3 origin)
    {
        multiSelect.SetActive(true);
        multiSelectImage.rectTransform.anchoredPosition = origin;
    }

    public void EndMultiSelect()
    {
        multiSelect.SetActive(false);
        //multiSelectImage.rectTransform.anchoredPosition = Vector2.zero;
        //multiSelectImage.rectTransform.sizeDelta = Vector2.zero;
    }

    public void DrawMultiSelect(Vector3 origMousePos, Vector3 currMousePos)
    {
     
        GameObject debugPrefab1 = GameObject.Instantiate(imagePrefab) as GameObject;
        debugPrefab1.transform.SetParent(this.transform);
        debugPrefab1.name = "1";

        GameObject debugPrefab2 = GameObject.Instantiate(imagePrefab) as GameObject;
        debugPrefab2.transform.SetParent(this.transform);
        debugPrefab2.name = "2";

        GameObject debugPrefab3 = GameObject.Instantiate(imagePrefab) as GameObject;
        debugPrefab3.transform.SetParent(this.transform);
        debugPrefab3.name = "3";

        GameObject debugPrefab4 = GameObject.Instantiate(imagePrefab) as GameObject;
        debugPrefab4.transform.SetParent(this.transform);
        debugPrefab4.name = "4";

        Vector3 diff = currMousePos - origMousePos;
        Vector3 rectOrigin = Vector3.zero;

        //Make the rect work for all quadrants

        Vector3 posBA = new Vector3(origMousePos.x + diff.x, origMousePos.y, 0);
        Vector3 posAB = new Vector3(origMousePos.x, origMousePos.y + diff.y, 0);
        
       

        debugPrefab1.GetComponent<Image>().rectTransform.position = origMousePos;
        debugPrefab2.GetComponent<Image>().rectTransform.position = currMousePos;
        debugPrefab3.GetComponent<Image>().rectTransform.position = posAB;
        debugPrefab4.GetComponent<Image>().rectTransform.position = posBA;

        if(diff.x > 0 && diff.y > 0)
        {
            //First Quadrant
            rectOrigin = origMousePos;
            
        }else if(diff.x < 0 && diff.y > 0)
        {
            //Second Quadrant
            rectOrigin = posBA;
        }else if(diff.x > 0 && diff.y < 0)
        {
            //Third Quadrant
            rectOrigin = posAB;
        }else if(diff.x < 0 && diff.y < 0)
        {
            //Fourth Quadrant
            rectOrigin = currMousePos;
        }
        
        Rect rect = new Rect(rectOrigin.x, rectOrigin.y, Mathf.Abs(diff.x), Mathf.Abs(diff.y));



        for (int i  = 0; i < player.selectableUnits.Count; ++i)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.selectableUnits[i].transform.position);

            if (rect.Contains(screenPos))
            {
                    //Debug.Log(multiSelectImage.rectTransform.rect.width);
                    player.selectableUnits[i].GetComponent<WorldObject>().SetSelection(true);
            }else
            {
                //Deselect - fix this logic
                //player.selectableUnits[i].GetComponent<WorldObject>().SetSelection(false);
            }
        }

    }

    public bool MouseInBounds(){

		Vector3 mousePos = Input.mousePosition;
		bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
		

		return insideWidth;
	}
}
