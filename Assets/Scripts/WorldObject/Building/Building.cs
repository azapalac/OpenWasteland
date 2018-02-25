using UnityEngine;
using System.Collections.Generic;

public class Building : WorldObject {

    public List<Resource> resourcesProduced;
    public int shelterProvided;
    public Well centerWell;
    public bool needsWaterRadius;
    public bool beingPlaced;
	// Use this for initialization
	protected override void Awake(){
		base.Awake ();

	}
	protected override void Start () {
		base.Start ();
        
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

	}
   
    public void Place()
    {
        //come back to this later
    }
}
