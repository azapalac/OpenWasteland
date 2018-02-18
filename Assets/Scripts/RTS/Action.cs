using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public enum ActionType
    {
        Default,
        Move,
        Attack,
        TakeDamage,
        Harvest,
        DropLoot,
        PickUpItems,
        BuildUnit,
        BuildStructure,
        Repair

    }

    public class ActionPackage
    {

    }
    public class MovePackage: ActionPackage
    {

    }

    public class Action
        {

            protected bool active;
            public bool Active { get { return active; } }
            public virtual ActionType Type { get { return ActionType.Default; } }
            public virtual void Execute(WorldObject worldObj)
            {
            if (!active)
            {
                return;
            }

            }
            //Dummy setup functions for override convenience

            public void Stop()
            {
                active = false;
            }

        }
    
    public enum Armor
    {
        Unarmored
    }
    //Upgrade and action specifics will be handled by blueprints, i.e. Workers can Harvest, Build Structures, Move, etc.
    public class Move : Action
        {
            public enum MoveType
            {
                Walk,
                Fly
            }

            float moveSpeed;
            MoveType moveType;
            Vector3 source, dest;
            public Move(float speed, MoveType type)
            {
                moveSpeed = speed;
                moveType = type;
            }
            public override ActionType Type { get { return ActionType.Move; } }


            public float MoveSpeed { get { return moveSpeed; } }

            public override void Execute(WorldObject worldObj)
            {
                if (active)
                {
                    //Move in a straight line for now
                    Vector3 delta = dest - source;
                    delta.Normalize();
                    worldObj.transform.Translate(delta * moveSpeed * Time.deltaTime);

                    if (Vector3.Magnitude(worldObj.transform.position - dest) < 1)
                    {
                        active = false;
                    }

                }
            }

            public  void SetUpMove( Vector3 source, Vector3 destination)
            {
                
                    this.source = source;
                    this.dest = destination;
                    active = true;
                    
               
            }


        }

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

    public class TakeDamage: Action
    {
        
        public override ActionType Type { get { return ActionType.TakeDamage; } }
        public List<Attack.AttackEffect> attackEffects; //need effect timers. There needs to be an AttackEffect class.
        
        private int hp;
        //handle armor
        private Armor armor;
        public int HP {  get { return hp; } }
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

                Stop();
            }

            for(int i  = 0; i < attackEffects.Count; i++)
            {
                //Deprecate timers on each AttackEffect. This will only fire if an attackEffect is present
            }
            
        }
    }
    
    public class Harvest: Action
    {
        private List<string> HarvestableObjects;

        private float harvestTime; //Add getters to these if we need to, currently unsure if necessary
        private float harvestTimer;
        private int harvestAmount;
        private WorldObject target;
        public override ActionType Type { get { return ActionType.Harvest; } }
        //There's only one harvest range, melee.

        public void SetUpHarvest(WorldObject harvestTarget)
        {
            harvestTimer = 0;
            target = harvestTarget;
        }

        public override void Execute(WorldObject worldObj)
        {
            if (target.CanDo(ActionType.DropLoot) && HarvestableObjects.Contains(target.objectName))
            {
              
                harvestTimer += Time.deltaTime;
                if(harvestTimer >= harvestTime)
                {
                    //Post loot drop event for target
                    harvestTimer = 0;

                }
            }
            else
            {
                //Post error message
            }
            
        }

        public Harvest(int harvestAmount, float harvestTime)
        {
            this.harvestAmount = harvestAmount;
            this.harvestTime = harvestTime;
        }
    }

    public class DropLoot: Action
    {
        public int lootDropThreshold;
        public List<Resource> lootToDrop;
        private int harvestDamageToTake;
        public override ActionType Type { get { return ActionType.DropLoot; } }
        //Doesn't matter if the loot drop is calculated when the object is created or when it is destroyed.
        //Every object can only drop once. Stretch goal - Minor loot drops when object is losing 
        public void SetUpDropLoot(int harvestDamage)
        {
            harvestDamageToTake = harvestDamage;
            active = true;
        }

        public override void Execute(WorldObject worldObj)
        {
            if (active)
            {
                lootDropThreshold -= harvestDamageToTake;
                if(lootDropThreshold <= 0)
                {
                    active = false;
                    DropAllResources();
                }
            }

        }

        public void DropAllResources()
        {
            //Drop all contained resources
        }

        public DropLoot(int threshold, List<Resource> loot)
        {
            lootToDrop = loot;
            lootDropThreshold = threshold;
        }
    }

    public class PickUpItems: Action
    {
        public Inventory inventory;
        public override ActionType Type { get { return ActionType.PickUpItems; } }
        private Resource resource;

        public void SetUpPickup(Resource resourceToPickUp)
        {
            resource = resourceToPickUp;
            active = true;
        }

        public override void Execute(WorldObject worldObj)
        {
            if (active)
            {
                inventory.AddResource(resource);

                Stop();
            }
        }

        public PickUpItems( int capacity)
        {
            inventory = new Inventory(capacity);
        }
        

    }
}