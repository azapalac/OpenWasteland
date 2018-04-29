using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvideWater : Property{
    public int waterToProvide;
    public float radius;
    public GameObject water;
    private bool showingWater;

	
	// Update is called once per frame
	void Update () {
        //Also activate this when placing a structure that requires it. 
        ShowWaterRadii(worldObject.Selected);
	}

    public void ShowWaterRadii(bool show)
    {
        water.SetActive(show);
        //Show/hide all other radii of water owned by this player
    }
}
