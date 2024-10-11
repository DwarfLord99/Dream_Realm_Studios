using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//file is pretty much the same as what we did in lecture with a small tweak to the enum
//comments are there to help everyone remember what everything does
public class Damage : MonoBehaviour
{
    [SerializeField] enum dmgType { bullet, melee }; //the damage types can be expanded upon later, added melee - RL 
    [SerializeField] dmgType type;
    [SerializeField] Rigidbody rb;

    //how fast, how strong, and how persistent (how long it stays instantiated) bullet is (respectively)
    [SerializeField] int dmgAmt; 
    [SerializeField] int speed;
    [SerializeField] int destroyTime; 
    void Start()
    {
        if (type == dmgType.bullet) //if it's a bullet, we give it velocity, destroy it after the time defined in the field
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        IDamage dmg = other.GetComponent<IDamage>();  //does the entity have the IDamage component?

        if (dmg != null && other.GetComponent<PlayerMovement>()) //y - damage entity = to the amount, added check to where enemies don't damage themselves - RL
        {
            dmg.takeDamage(dmgAmt);
        }

        if (type == dmgType.bullet) //n - destroy the object (collision occured but no damage was taken, so free up memory)
        {
            Destroy(gameObject);
        }
    }

}
