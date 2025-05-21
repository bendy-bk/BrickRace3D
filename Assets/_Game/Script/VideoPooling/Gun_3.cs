using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_3 : GunBase
{
    [SerializeField] Transform bulletPoint;
    [SerializeField] BulletBase bulletBasePrefab;

    public override void Shoot()
    {
        base.Shoot();
        StartCoroutine(IEShoot());
        
    }


    private IEnumerator IEShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            //BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation);
            BulletBase b = SimplePool.Spawn<BulletBase>(PoolType.Bullet_3, bulletPoint.position, bulletPoint.rotation);
            b.OnInit(10);
            yield return new WaitForSeconds(0.1f);
        }
       
    }
}
