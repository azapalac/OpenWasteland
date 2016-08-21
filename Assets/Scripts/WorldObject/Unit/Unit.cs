using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RTS;
public class Unit : WorldObject {

	protected override void Awake(){

		base.Awake();
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();
        ActionManager.AddAction(actions, "Move");
        ActionManager.AddAction(actions, "Harvest Junk");
        ActionManager.AddAction(actions, "Pick Up Resources");
    }



    // Update is called once per frame
    protected override void Update () {
        
		base.Update();
	}


}
