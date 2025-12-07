using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : Plant
{
    private List<Bullet> ignitedBullets; // 已点燃的子弹

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Torchwood;
        type = PlantType.Normal;
        ignitedBullets = new List<Bullet>();
    }

    public override void OnChildTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.bullet)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet && CanIgnite(bullet) && !ignitedBullets.Contains(bullet))
            {
                ignitedBullets.Add(bullet);
                Bullet newBullet = bullet.Ignite();
                if (newBullet) ignitedBullets.Add(newBullet);
            }
        }
    }

    public override void OnChildTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.bullet)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet && ignitedBullets.Contains(bullet))
            {
                ignitedBullets.Remove(bullet);
            }
        }
    }

    private bool CanIgnite(Bullet bullet)
    {
        return bullet.targetRows.Contains(0) || bullet.targetRows.Contains(row);
    }
}
