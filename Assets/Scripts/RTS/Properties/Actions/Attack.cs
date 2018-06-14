using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attack : Action
{
    public enum Range
    {
        Melee, Short, Medium, Long, Test
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

    public GameObject projectilePrefab;

    //For now, projectiles move in a straight line
    public float projectileSpeed;
    

    private float attackTimer;

    [SerializeField]
    public Range AttackRange;


    public bool attackWhileMoving;

    [SerializeField]
    public AttackEffect attackEffect;

    //Change to projectile based eventually
    private WorldObject target;

    private float AttackRangeVal
    {
        get
        {
            switch (AttackRange)
            {
                case Range.Melee:
                    return 0.5f;
                case Range.Short:
                    return 10f;
                case Range.Medium:
                    return 30f;
                case Range.Long:
                    return 50f;
                case Range.Test:
                    return Mathf.Infinity;

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
            if(dist > AttackRangeVal)
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
              
                worldObject.StartDoing(this);

                if (!attackWhileMoving)
                {
                    worldObject.StopDoing(ActionType.Move);
                }
            }
        }
        else
        {
            
        }

      
    }



 
    public override void Execute(WorldObject worldObj)
    {
        if(target == null)
        {
            //Stop attacking if the target is destroyed
            Stop();
        }

        if (active)
        {


            if (!target.CanDo(ActionType.TakeDamage))
            {
                //Post invalid message
                Stop();
            }
            //The higher the attack speed, the faster the worldObject attacks
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackSpeed)
            {

                if(AttackRange == Range.Melee)
                {
                    //Play animation
                    //Set up the take damage effect
                    TakeDamage takeDamage = target.GetComponent<TakeDamage>();
                    takeDamage.SetUpTakeDamage(attackDamage, attackEffect);
                }
                else
                {
                    //play animation
                    //Launch a projectile that applies the damage effect
                    GameObject g = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                    Projectile projectile = g.GetComponent<Projectile>();
                    projectile.effect = attackEffect;
                    projectile.damage = attackDamage;
                    projectile.speed = projectileSpeed;
                    projectile.range = AttackRangeVal;
                    projectile.target = target;
                    projectile.parent = gameObject;
                }
                //Reset timer
                attackTimer = 0;
                
            }

        }

    }
    
    //Apply an attack effect to a target
    private void ApplyEffect()
    {
        //Status effects to be added in later. Right now everything is "Normal"
    }
}

