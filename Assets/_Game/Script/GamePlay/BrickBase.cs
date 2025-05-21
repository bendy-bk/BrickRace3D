using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBase : GameUnit
{
    public void OnDesSpawn()
    {
        SimplePool.Despawn(this);
        //Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        //TODO: set damage
    //        OnDesSpawn();
    //    }
    //    else
    //    {
    //        if (other.CompareTag("Enemy"))
    //        {
    //            OnDesSpawn();
    //        }

    //    }

    //}

}
