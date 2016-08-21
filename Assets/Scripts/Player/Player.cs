﻿using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	public string username;
	public bool isHuman;
	public HUD hud;
	public bool objectIsSelected = false;
	public WorldObject SelectedObject { get; set;}

    public enum status
    {
        NotFound,
        Neutral,
        Ally,
        Enemy

    };
    public Dictionary<string, status> knownPlayers;
    public void AddKnownPlayer(string playername)
    {
        //All players start out as neutral to each other
        knownPlayers.Add(playername, status.Neutral);
    }

    public status GetStatus(string playername)
    {
        if (knownPlayers.ContainsKey(playername))
        {
            return knownPlayers[playername];
        }
        AddKnownPlayer(playername);
        return knownPlayers[playername];
    }
	// Use this for initialization
	void Start () {
        knownPlayers = new Dictionary<string, status>();
		hud = GetComponentInChildren<HUD>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}
	
}
