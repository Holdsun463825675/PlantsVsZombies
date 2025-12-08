using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObstacle : MonoBehaviour
{
    public BulletType obstacleBulletType;
    public BulletDirection obstacleBulletDirection;

    private ZombieUnderAttackSound underAttackSound;
    private int underAttackSoundPriority;

    private void Awake()
    {
        underAttackSound = ZombieUnderAttackSound.Splat;
        underAttackSoundPriority = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.bullet)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet && bullet.bulletType == obstacleBulletType && bullet.bulletDirection == obstacleBulletDirection)
            {
                if (bullet.BulletHitPrefab) GameObject.Instantiate(bullet.BulletHitPrefab, bullet.transform.position, Quaternion.identity);
                AudioManager.Instance.playHitClip(bullet.hitSound, bullet.hitSoundPriority, underAttackSound, underAttackSoundPriority);
                bullet.setState(BulletState.Used);
            } 
        }
    }
}
