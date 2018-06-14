using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TakeDamage : Action
{

    public override ActionType Type { get { return ActionType.TakeDamage; } }
    private List<Attack.AttackEffect> attackEffects; //need effect timers. There needs to be an AttackEffect class.

    public int maxHP;
    public int hp { get; private set; }

    //handle armor
    [SerializeField]
    public Armor armor;


    public HP_Bar hpBar;

    private int damageToTake;
    private Attack.AttackEffect effectToTake;

    public void Start()
    {
        hp = maxHP;
        attackEffects = new List<Attack.AttackEffect>();
    }

    public void SetUpTakeDamage(int damage, Attack.AttackEffect effect)
    {
        damageToTake = damage;
        this.effectToTake = effect;
        active = true;
        worldObject.StartDoing(this);
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
            //worldObj.TriggerDropLoot(DropLoot.ObjectDestroyed);


            if(hp <= 0)
            {
                if (worldObj.CanDo(ActionType.DropLoot))
                {
                    worldObj.GetComponent<DropLoot>().SetUpDropLoot();
                }
                Destroy(this.gameObject);
            }
            //Debug.Log("Ouch! " + hp + "/" + maxHP + " HP remaining.");
            hpBar.SetPercentage(hp, maxHP);

            Stop();
        }

        for (int i = 0; i < attackEffects.Count; i++)
        {
            //Deprecate timers on each AttackEffect. This will only fire if an attackEffect is present
        }

    }

    public bool HasEffect(Attack.AttackEffect effect)
    {
        if (attackEffects != null)
        {
            return attackEffects.Contains(effect);
        }
        else
        {
            return false;
        }
    }

    public void AddEffect(Attack.AttackEffect effect)
    {
        attackEffects.Add(effect);
    }

    public void RemoveEffect(Attack.AttackEffect effect)
    {
        if (attackEffects.Contains(effect))
        {
            attackEffects.Remove(effect);
        }
    }
}