﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RTS;
public class Unit : WorldObject {
    public int populationCost;
    public string unitType { get; set; }
	protected override void Awake(){

		base.Awake();
	}
	// Use this for initialization
	protected override void Start () {
		base.Start();
        //Units are group selectable
        owner.selectableUnits.Add(this.gameObject);
    }



    // Update is called once per frame
    protected override void Update () {
        
		base.Update();
	}


}
