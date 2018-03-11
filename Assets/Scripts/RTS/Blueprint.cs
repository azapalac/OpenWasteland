using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

    //PROBLEM - HOW TO CONSTRUCT UNITS??? Does BlueprintManager now reference itself?
    public abstract class Blueprint
    {

        public string Name{ get; set; }
        public float ConstructionTime { get; set; }

        public int TechLevel { get; set; }
        public List<Resource> Ingredients {get; set;}
        public int KnowledgePoints { get; set;}
        public List<Action> DefaultActions { get; set; }
        
        
    }

    public class ConstructionBlueprint: Blueprint
    {

        public GameObject product { get; set; }

        public int shelterProvided { get; set; }
    }

    public class CraftingBlueprint: Blueprint
    {
        public Resource Product { get; set; }
    }

    public class UnitBlueprint : Blueprint
    {
        public int populationCost { get; set; }
       
        public GameObject product { get; set; }
       
    }

    
    public class BlueprintManager
    {
        private static KeyValuePair<string, ConstructionBlueprint> MakeBlueprint(
            string name, int populationCost, int techLevel, List<Resource> ingredients, float constructionTime)
        {
            return new KeyValuePair<string, ConstructionBlueprint>(name, new ConstructionBlueprint
            {
                Name = name,
                TechLevel = techLevel,
                Ingredients = ingredients,
                product = PrefabLoader.LoadStructure(name),
                ConstructionTime = constructionTime

            });
        }

        private static KeyValuePair<string, CraftingBlueprint> MakeBlueprint(
           string name, int techLevel, List<Resource> ingredients, float constructionTime, Resource product)
        {
            return new KeyValuePair<string, CraftingBlueprint>(name, new CraftingBlueprint
            {
                Name = name,
                TechLevel = techLevel,
                Ingredients = ingredients,
                Product = product,
                ConstructionTime = constructionTime
            });
        }

    public static Dictionary<string, Blueprint> BaseBlueprintDictionary;
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

