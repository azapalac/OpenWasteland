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
    public MoveType moveType;
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

    public void SetUpMove(Vector3 source, Vector3 destination)
    {

        this.source = source;
        this.dest = destination;
        active = true;


    }


}