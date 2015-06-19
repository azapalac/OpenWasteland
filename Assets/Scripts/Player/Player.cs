using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string username;
	public bool isHuman;
	public HUD hud;
	public bool objectIsSelected = false;
	public WorldObject SelectedObject { get; set;}



	// Use this for initialization
	void Start () {
		hud = GetComponentInChildren<HUD>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}
	
}
