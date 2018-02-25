using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : Action
{
    public enum Range
    {
        Melee, Short, Medium, Long
    }
    public override ActionType Type
    {
        get
        {
            return ActionType.Attack;
        }
    }
    public enum AttackEffect
    {
        Normal, Burning, Cold, Acid, Laser, Electric, Pierce
    }
    private int attackDamage;
    public int AttackDamage { get { return attackDamage; } }
    private float attackSpeed;
    private float attackTimer;
    public float AttackSpeed { get { return attackSpeed; } }
    private Range attackRange;
    private AttackEffect attackEffect;

    //Change to projectile based eventually
    private WorldObject target;

    public float AttackRange
    {
        get
        {
            switch (attackRange)
            {
                case Range.Melee:
                    return 0.5f;
                case Range.Short:
                    return 10f;
                case Range.Medium:
                    return 30f;
                case Range.Long:
                    return 50f;
                default:
                    return -1;
            }
        }
    }

    public Attack(int damage, float speed, Range range, AttackEffect effect)
    {
        attackDamage = damage;
        attackSpeed = speed;

        attackEffect = effect;
        attackRange = range;

    }


    public void SetUpAttack(WorldObject target)
    {
        attackTimer = 0;
        this.target = target;
        active = true;
    }
    public override void Execute(WorldObject worldObj)
    {
        if (active)
        {

            if (!target.CanDo(ActionType.TakeDamage))
            {
                //Post invalid message
                Stop();
            }
            //The higher the attack speed, the faster the worldObject attacks
            if (target.CanDo(ActionType.TakeDamage) && attackTimer >= 1 / attackSpeed)
            {
                target.hitPoints -= attackDamage;
                //TODO: post TakeDamage event to the class

                //TODO: Fire projectile and do attack animation. Projectile should do damage, not the function.
                ApplyEffect();
                attackTimer += Time.deltaTime;
            }


        }
    }
    //Apply an attack effect to a target
    private void ApplyEffect()
    {
        //Status effects to be added in later. Right now everything is "Normal"
    }
}
