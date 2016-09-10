using UnityEngine;
using System.Collections.Generic;
using RTS;
public class Player : MonoBehaviour {

	public string username;
	public bool isHuman;
	public HUD hud;
	public bool objectIsSelected = false;
	public WorldObject SelectedObject { get; set;}
    public Dictionary<string, Blueprint> knownBlueprints;
    public int knowledgeLimit;
    public List<GameObject> selectableUnits;

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
    void Awake()
    {
        //selectableUnits = new List<GameObject>();
    }
	void Start () {
        knownPlayers = new Dictionary<string, status>();
        
		hud = GetComponentInChildren<HUD>();
	}
	
	// Update is called once per frame
	void Update () {
	

	}
	
}
