using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

namespace RTS
{
    public abstract class Blueprint
    {

        public string Name{ get; set; }
        public float ConstructionTime { get; set; }
        public int TechLevel { get; set; }
        public List<Resource> Ingredients {get; set;}
        public int KnowledgePoints { get; set;}

    }

    public class ConstructionBlueprint: Blueprint
    {
        //For buildings, the population cost will just be zero
        public int populationCost { get; set; }
        public GameObject product { get; set; }
    }

    public class CraftingBlueprint: Blueprint
    {
        public Resource Product { get; set; }
    }




    public class BlueprintManager
    {
        private static KeyValuePair<string, ConstructionBlueprint> MakeBlueprint(
            string name, int populationCost, int techLevel, List<Resource> ingredients)
        {
            return new KeyValuePair<string, ConstructionBlueprint>(name, new ConstructionBlueprint
            {
                Name = name,
                TechLevel = techLevel,
                populationCost = populationCost,
                Ingredients = ingredients,
                product = GameManager.WorldObjectDictionary[name]

            });
        }

        private static KeyValuePair<string, CraftingBlueprint> MakeBlueprint(
           string name,  int techLevel, List<Resource> ingredients, Resource product)
        {
            return new KeyValuePair<string, CraftingBlueprint>(name, new CraftingBlueprint
            {
                Name = name,
                TechLevel = techLevel,
                Ingredients = ingredients,
                Product = product

            });
        }
        /*
           new List<Resource> {
                        ObjectManager.GetResource(20,"Scrap"),
                        ObjectManager.GetResource(5, "Stone")

        */

        public static Dictionary<string, Blueprint> BaseBlueprintDictionary;
        public static void SetUpBlueprintDictionary()
        {
            BaseBlueprintDictionary = new Dictionary<string, Blueprint>();
            KeyValuePair<string, ConstructionBlueprint> constructionKeyValue;
            KeyValuePair<string, CraftingBlueprint> craftingKeyValue;

            //Example of a unit construction blueprint
                constructionKeyValue =  MakeBlueprint("Harvester", 1, 0, 
              new List<Resource> {
                        ObjectManager.GetResource(10,"Scrap"),
                        ObjectManager.GetResource(5, "Stone") } );
            BaseBlueprintDictionary.Add(constructionKeyValue.Key, constructionKeyValue.Value);

            //
            craftingKeyValue = MakeBlueprint("Iron", 0,
            new List<Resource> {
                ObjectManager.GetResource(20,"Scrap")
            },
             ObjectManager.GetResource(5, "Iron"));
            BaseBlueprintDictionary.Add(craftingKeyValue.Key, craftingKeyValue.Value);
        }

    }
}
