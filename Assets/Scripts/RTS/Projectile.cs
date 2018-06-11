using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public WorldObject target;
    public int damage;
    public Attack.AttackEffect effect;
    public float speed;
    public float range;
    private Vector3 direction;
    private Vector3 distance;
    public GameObject parent;
	// Use this for initialization
	void Start () {
        Vector3 source = transform.position;
        Vector3 dest = target.transform.position;
        direction = dest - source;
        direction.Normalize();
        distance = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {
        //Move in a straight line to where the target is, at a high speed. Has a large hitbox
        Vector3 deltaMovement = direction * speed * Time.deltaTime;
        transform.Translate(deltaMovement);
        distance += deltaMovement;
        if(Vector3.Magnitude(distance) >= range)
        {
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider other)
    {
        WorldObject worldObject = other.gameObject.GetComponent<WorldObject>();
        if ( worldObject != null && other.gameObject != parent)
        {
            

            if (worldObject.CanDo(ActionType.TakeDamage))
            {
                worldObject.GetComponent<TakeDamage>().SetUpTakeDamage(damage, effect);
            }
           // Destroy(gameObject);
        }

        
    }
}
