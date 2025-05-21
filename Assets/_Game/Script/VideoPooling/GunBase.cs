using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    [SerializeField] float frameRate = 1f;
    float time = 0f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= frameRate)
        {
            time -= frameRate;
            Shoot();
        }
       
    }

    public virtual void Shoot()
    {

    }

}
