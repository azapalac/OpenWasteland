using UnityEngine;
using System.Collections.Generic;
using RTS;
public class GameManager : MonoBehaviour {
    
    
    public GameObject[] prefabList;
    public static GameObject[] ResourcePackPrefabs;

    void Awake()
    {
        ResourcePackPrefabs = prefabList;
        //Initialize all prefabs
        for(int i = 0; i < ResourcePackPrefabs.Length; i++)
        {
            ResourcePackPrefabs[i].GetComponent<ResourcePack>().Initialize();
        }
    }

    private static  Dictionary<string, GameObject> GetMap()
    {
        Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();
        
        for(int i = 0; i < ResourcePackPrefabs.Length; i++)
        {
            ResourcePack r = ResourcePackPrefabs[i].GetComponent<ResourcePack>();
            map.Add(ResourcePackPrefabs[i].GetComponent<ResourcePack>().containedResource.name, ResourcePackPrefabs[i]);
        }
        return map;
    }

    public static Dictionary<string, GameObject> PrefabDictionary
    {
        get
        {
            return GetMap();
        }
    }
}

