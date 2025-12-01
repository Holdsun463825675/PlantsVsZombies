using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchwood : Plant
{
    public string ignitePlaceName = "IgnitePlace";
    protected Collider2D ignitePlaceCollider;
    private List<Bullet> ignitedBullets; // 已点燃的子弹

    protected override void Awake()
    {
        base.Awake();
        id = PlantID.Torchwood;
        type = PlantType.Normal;
        ignitedBullets = new List<Bullet>();
        // 设置子物体碰撞器
        ignitePlaceCollider = transform.Find(ignitePlaceName).GetComponent<Collider2D>();
        ignitePlaceCollider.GetComponent<TriggerForwarder>().SetPlantParentHandler(this);
        ignitePlaceCollider.enabled = false;
    }

    public override void setState(PlantState state)
    {
        base.setState(state);
        switch (state)
        {
            case PlantState.Suspension:
                ignitePlaceCollider.enabled = false;
                break;
            case PlantState.Idle:
                ignitePlaceCollider.enabled = true;
                break;
            case PlantState.Die:
                ignitePlaceCollider.enabled = false;
                break;
            default:
                break;
        }
    }

    public override void OnChildTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.bullet)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet && !ignitedBullets.Contains(bullet))
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
}
