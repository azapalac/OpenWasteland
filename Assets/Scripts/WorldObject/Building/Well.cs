using UnityEngine;
using System.Collections;
using RTS;
public class Well : Building {
    private GameObject radius;
    public float radiusScale;
    protected override void Start()
    {
        base.Start();
        radius = Instantiate(ResourceManager.GetRadius, transform.position, Quaternion.identity) as GameObject;
        radius.transform.Rotate(new Vector3(90, 0, 0));
        radius.transform.parent = this.transform;
        radius.transform.localScale *= radiusScale;
        blueprintLimit = 2;
        ActionManager.AddAction(actions, "Construct Unit");
        this.LoadBlueprint(BlueprintManager.BaseBlueprintDictionary["Harvester"]);
    }
    public override void DrawSelection()
    {
        //Make a better color later
        SpriteRenderer radiusRenderer = radius.GetComponent<SpriteRenderer>();
        Color radiusColor = Color.blue;
        radiusColor.a = 0.5f;
        if (currentlySelected)
        {
            //TODO: Change this to be red for enemies and green for allies
            radiusRenderer.color = radiusColor;
            selectionBoxRenderer.color = Color.black;
        }
        else {
            radiusRenderer.color = Color.clear;
            selectionBoxRenderer.color = Color.clear;
        }
    

    }
}
