using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

    //PROBLEM - HOW TO CONSTRUCT UNITS??? Does BlueprintManager now reference itself?
    public class Blueprint: MonoBehaviour
    {


    public float constructionTime;

    public int techLevel;

    [SerializeField]
    public List<ResourceType> ingredients;

    public int knowledgePoints;

    public GameObject product;
    }



    public class PrefabLoader
    {
        public static GameObject LoadStructure (string prefabName){

           return Resources.Load("Structures/"+ prefabName) as GameObject;
            
        }

        public static GameObject LoadUnit(string prefabName)
        {
            return Resources.Load("Units/" + prefabName) as GameObject;
        }

    }

