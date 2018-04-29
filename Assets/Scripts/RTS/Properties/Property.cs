using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Property : MonoBehaviour {
    protected WorldObject worldObject;
    protected virtual void Awake()
    {
        //check for attached worldObject, throw an error if not there
        worldObject = GetComponent<WorldObject>();
        if (worldObject == null)
        {
            Debug.LogError("ERROR: WorldObject script not attached!");

        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
