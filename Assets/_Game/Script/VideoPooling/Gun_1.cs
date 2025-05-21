using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_1 : GunBase
{

    [SerializeField] Transform bulletPoint;
    [SerializeField] BulletBase bulletBasePrefab;

    public override void Shoot()
    {
        base.Shoot();
        //BulletBase b = Instantiate(bulletBasePrefab, bulletPoint.position, bulletPoint.rotation);
        BulletBase b = SimplePool.Spawn<BulletBase>(PoolType.Bullet_1, bulletPoint.position, bulletPoint.rotation);
        b.OnInit(10);
    }

}
