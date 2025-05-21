using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : GameUnit
{

    [SerializeField] private float speed = 10;
    float damage;

    private void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
    }

    public void OnInit(float damage)
    {
        this.damage = damage;
    }
    public void OnDesSpawn()
    {
        SimplePool.Despawn(this);
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            //TODO: set damage
            OnDesSpawn();
        }
    }

}
