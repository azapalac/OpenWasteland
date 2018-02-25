using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TakeDamage : Action
{

    public override ActionType Type { get { return ActionType.TakeDamage; } }
    public List<Attack.AttackEffect> attackEffects; //need effect timers. There needs to be an AttackEffect class.

    private int hp;
    //handle armor
    private Armor armor;
    public int HP { get { return hp; } }
    private int damageToTake;
    private Attack.AttackEffect effectToTake;

    public TakeDamage(int hitPoints, Armor armorType)
    {
        //Have an override for Shields eventually
        //Each unit can only have one armor type
        hp = hitPoints;
        armor = armorType;
    }
    public void SetUpTakeDamage(int damage, Attack.AttackEffect effect)
    {
        damageToTake = damage;
        this.effectToTake = effect;
        active = true;
    }

    //Handle healing eventually

    public override void Execute(WorldObject worldObj)
    {
        if (active)
        {
            //Armor math goes here
            switch (armor)
            {
                case Armor.Unarmored:
                    hp -= damageToTake;
                    attackEffects.Add(effectToTake);
                    break;
            }
            //-1 counts as a critical success so it can bypass the harvest threshold
            worldObj.TriggerDropLoot(DropLoot.ObjectDestroyed);

            Stop();
        }

        for (int i = 0; i < attackEffects.Count; i++)
        {
            //Deprecate timers on each AttackEffect. This will only fire if an attackEffect is present
        }

    }
}