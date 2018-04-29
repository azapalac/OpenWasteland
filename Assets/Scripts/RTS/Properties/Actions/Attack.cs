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

    public int attackDamage;
    public float attackSpeed;



    private float attackTimer;

    [SerializeField]
    public Range attackRange;

    [SerializeField]
    public AttackEffect attackEffect;

    //Change to projectile based eventually
    private WorldObject target;

    private float AttackRangeVal
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

    


    public override void SetUpRightClick(Vector3 hitPoint, GameObject clickedObject)
    {
        //If there is a target within range, attack it, otherwise queue up the attack
        WorldObject target = clickedObject.GetComponent<WorldObject>();
        float dist = Vector3.Distance(transform.position, hitPoint);
        if (target != null)
        {
            attackTimer = 0;
            this.target = target;
            active = true;
            if(dist < AttackRangeVal)
            {
                //Queue up an attack
                worldObject.QueueAction(this);

                //SetUp a move action
                if (worldObject.CanDo(ActionType.Move))
                {
                    worldObject.GetComponent<Move>().SetUpRightClick(hitPoint, clickedObject);
                }
            }
            else
            {

            }
        }
        else
        {
            
        }

      
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

                //TODO: post TakeDamage event to the class
                //target.TriggerTakeDamage(attackDamage, attackEffect);
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
