using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Move : Action
{
    public enum MoveType
    {
        Walk,
        Fly
    }

    public float moveSpeed;

    [SerializeField]
    public MoveType moveType;
    Vector3 source, dest;
   
    public override ActionType Type { get { return ActionType.Move; } }

    public override void Execute(WorldObject worldObj)
    {
        
        if (active)
        {
            //Move in a straight line for now
            Vector3 delta = dest - source;
            delta.Normalize();
            transform.Translate(delta * moveSpeed * Time.deltaTime);

            if (Vector3.Magnitude(transform.position - dest) < 2*moveSpeed*Time.deltaTime)
            {
                active = false;
                worldObject.StopDoing(this);
            }

        }
    }

    //Add queue system later
    public override void SetUpRightClick(Vector3 destination, GameObject clickedObject)
    {
        //Make sure to unload this from the queue so I don't stack up multiples of the same command
        if (worldObject.IsDoing(this))
        {
            worldObject.StopDoing(this);
            if (worldObject.CanDo(ActionType.Attack))
            {
                if (!worldObject.GetComponent<Attack>().attackWhileMoving)
                {
                    worldObject.StopDoing(ActionType.Attack);
                }
            }
        }

        source = transform.position;
        dest = destination;
        active = true;
        worldObject.StartDoing(this);

    }


}