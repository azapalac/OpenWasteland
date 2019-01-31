using UnityEngine;
using System.Collections.Generic;


public class Player : MonoBehaviour {

	public string username;
	public bool isHuman;
	public HUD hud;
	public bool objectIsSelected = false;
	public WorldObject SelectedObject { get; set;}
    public Dictionary<string, Blueprint> knownBlueprints;
    public List<ResourceInfo> storedResources;
    public int knowledgeLimit;
    public List<GameObject> selectableUnits;

    public List<WorldObject> SelectedObjects;



    public enum Status
    {
        NotFound,
        Neutral,
        Ally,
        Enemy

    };
    public Dictionary<string, Status> knownPlayers;
    public void AddKnownPlayer(string playername)
    {
        //All players start out as neutral to each other
        knownPlayers.Add(playername, Status.Neutral);
    }

    public Status GetStatus(string playername)
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
        SelectedObjects = new List<WorldObject>();
        //selectableUnits = new List<GameObject>();
    }
	void Start () {
        knownPlayers = new Dictionary<string, Status>();
        storedResources = new List<ResourceInfo>();
        knownBlueprints = new Dictionary<string, Blueprint>();
		hud = GetComponentInChildren<HUD>();
	}
	
    //Returns true if at least one object is selected
    public bool ObjectSelected()
    {
        return SelectedObjects.Count != 0;
    }


	// Update is called once per frame
	void Update () {
	

	}
	
}
