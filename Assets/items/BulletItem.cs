using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletItem : Item
{
    public abstract Bullet transformBullet(Bullet bullet);
}
