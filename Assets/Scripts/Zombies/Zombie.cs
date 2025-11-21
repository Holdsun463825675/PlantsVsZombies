using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ZombieID
{
    None,
    NormalZombie,
    FlagZombie,
}

public enum ZombieMoveState
{
    Move,
    Stop,
}

public enum ZombieHealthState
{
    Healthy,
    LostArm,
    LostHead,
    Die
}

public class Zombie : MonoBehaviour, IClickable
{
    // 需继承实现
    public ZombieID zombieID;
    protected float base_x_Speed;
    protected float x_Speed, x_speed, y_Speed, y_speed;
    protected int maxHealth, currHealth;
    protected int maxArmor1Health, currArmor1Health;
    protected int maxArmor2Health, currArmor2Health;
    protected int attackPoint;
    protected bool isLostHealth;
    public float spawnWeight;
    public GameObject zombieHeadPrefab;
    private GameObject zombieHead;

    private float groanTime, groanTimer;
    private float HealthPercentage;
    private int dieMode;
    protected float speedLevel;
    private Transform losingGame;

    private ZombieMoveState moveState;
    private ZombieHealthState healthState;

    private TextMeshPro HPText;
    private Transform lostHeadPlace;
    private List<Plant> targets;
    private Animator lostHeadAnim;

    private Animator anim;
    private Collider2D c2d;

    protected virtual void Awake()
    {
        zombieID = ZombieID.None;
        base_x_Speed = 0.2f;
        x_Speed = -Random.Range(1.0f, 2.0f) * base_x_Speed;
        x_speed = 0.0f;
        maxHealth = 270; currHealth = maxHealth;
        maxArmor1Health = 0; currArmor1Health = maxArmor1Health;
        maxArmor2Health = 0; currArmor2Health = maxArmor2Health;
        attackPoint = 50;
        isLostHealth = false;
        spawnWeight = 1.0f;

        speedLevel = (-x_Speed - base_x_Speed) / base_x_Speed;
        HealthPercentage = 1.0f;
        groanTime = 23.0f + Random.Range(0.0f, 2.0f); groanTimer = 23.0f;
        dieMode = 0;
        moveState = ZombieMoveState.Stop;
        targets = new List<Plant>();
        anim = GetComponent<Animator>();
        anim.SetBool(AnimatorConfig.zombie_game, false);
        c2d = GetComponent<Collider2D>();
        c2d.enabled = false;
        Transform child = transform.Find("HPText");
        if (child) HPText = child.GetComponent<TextMeshPro>();
        if (HPText) HPText.gameObject.SetActive(true);
        child = transform.Find("LostHeadPlace");
        if (child) lostHeadPlace = child.GetComponent<Transform>();
    }

    void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = true;

        losingGame = MapManager.Instance.currMap.endlinePositions[0];
    }

    void FixedUpdate()
    {
        HPText.text = $"{currHealth}/{maxHealth}";
        HPText.gameObject.SetActive(SettingSystem.Instance.settingsData.zombieHealth);
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        HealthPercentage = (float)currHealth / (float)maxHealth;
        anim.SetFloat(AnimatorConfig.zombie_healthPercentage, HealthPercentage);
        anim.SetFloat(AnimatorConfig.zombie_speedLevel, speedLevel);
        if (HealthPercentage >= 0.666f) setHealthState(ZombieHealthState.Healthy);
        if (HealthPercentage >= 0.333f && HealthPercentage < 0.666f) setHealthState(ZombieHealthState.LostArm);
        if (HealthPercentage >= 0.001f && HealthPercentage < 0.333f) setHealthState(ZombieHealthState.LostHead);
        if (HealthPercentage < 0.001f) setHealthState(ZombieHealthState.Die);

        switch (healthState)
        {
            case ZombieHealthState.Healthy:
                HealthyUpdate(); break;
            case ZombieHealthState.LostArm:
                LostArmUpdate(); break;
            case ZombieHealthState.LostHead:
                LostHeadUpdate(); break;
            case ZombieHealthState.Die:
                DieUpdate(); break;
            default:
                break;
        }
    }

    public void setGameMode()
    {
        c2d.enabled = true;
        anim.SetBool(AnimatorConfig.zombie_game, true);
    }

    // 暂停继续功能
    public void Pause()
    {
        anim.enabled = false;
        if ((healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) && lostHeadAnim) lostHeadAnim.enabled = false;
    }

    public void Continue()
    {
        anim.enabled = true;
        if ((healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) && lostHeadAnim) lostHeadAnim.enabled = true;
    }

    public void OnClick()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.plant)
        {
            Plant target = collision.GetComponent<Plant>();
            if (target && !targets.Contains(target)) targets.Add(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagConfig.plant)
        {
            Plant target = collision.GetComponent<Plant>();
            targets.Remove(target);
        }
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getCurrHealth()
    {
        return currHealth;
    }

    public int setSortingOrder(int sortingOrder)
    {
        int count = 0;
        this.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder; count++;
        if (HPText) HPText.sortingOrder = sortingOrder + ++count; count++;
        return count;
    }

    public void setMoveState(ZombieMoveState state)
    {
        if (state == ZombieMoveState.Move)
        {
            x_speed = x_Speed;
        }
        if (state == ZombieMoveState.Stop)
        {
            x_speed = 0.0f;
        }
        this.moveState = state;
    }

    public void setHealthState(ZombieHealthState state)
    {
        if (this.healthState == state) return;
        this.healthState = state;
        if (state == ZombieHealthState.Healthy)
        {
            
        }
        if (state == ZombieHealthState.LostArm)
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_zombiedie_limbsPop);
        }
        if (state == ZombieHealthState.LostHead)
        {
            zombieHead = GameObject.Instantiate(zombieHeadPrefab, lostHeadPlace.position, Quaternion.identity);
            zombieHead.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
            lostHeadAnim = zombieHead.GetComponent<Animator>();
            AudioManager.Instance.playClip(ResourceConfig.sound_zombiedie_limbsPop);
        }
        if (state == ZombieHealthState.Die)
        {
            c2d.enabled = false;
            anim.SetInteger(AnimatorConfig.zombie_dieMode, dieMode);
        }
    }

    private void kinematicsUpdate()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false) return;
        if (targets.Count != 0)
        {
            anim.SetBool(AnimatorConfig.zombie_isAttack, true);
            return;
        }
        anim.SetBool(AnimatorConfig.zombie_isAttack, false);

        Vector3 newPosition = transform.position;
        newPosition.x += x_speed * Time.fixedDeltaTime;
        newPosition.y += y_speed * Time.fixedDeltaTime;
        transform.position = newPosition;
    }

    private void groanUpdate()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false || healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) return;
        // 僵尸叫声
        groanTimer += Time.fixedDeltaTime;
        if (groanTimer >= groanTime)
        {
            int idx = Random.Range(0, ResourceConfig.sound_other_groans.Length);
            AudioManager.Instance.playClip(ResourceConfig.sound_other_groans[idx]);
            groanTimer = 0.0f;
            groanTime = 23.0f + Random.Range(0.0f, 2.0f);
        }
    }

    public void AddHealth(int point)
    {
        currHealth += point;
        if (currHealth > maxHealth) currHealth = maxHealth;
        if (currHealth <= 0) currHealth = 0;
    }

    public void UnderAttack(int point, int mode=0)
    {
        dieMode = mode;
        anim.SetBool(AnimatorConfig.zombie_underAttack, true);
        AddHealth(-(int)((float)point * SettingSystem.Instance.settingsData.hurtRate)); // 根据受伤比例计算伤害
    }

    protected virtual void Attack()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false || healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) return;
        if (targets.Count != 0)
        {
            int idx = Random.Range(0, ResourceConfig.sound_zombieeat_chomps.Length);
            Plant target = targets[0];
            target.UnderAttack(attackPoint);
            if (target == null || target.getState() == PlantState.Die) AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_gulp);
            else AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_chomps[idx]);
        }
    }

    private void HealthyUpdate()
    {
        kinematicsUpdate();
        groanUpdate();
        if (transform.position.x <= losingGame.position.x) GameManager.Instance.setState(GameState.Losing);
    }

    private void LostArmUpdate()
    {
        kinematicsUpdate();
        groanUpdate();
        if (transform.position.x <= losingGame.position.x) GameManager.Instance.setState(GameState.Losing);
    }

    private void LostHeadUpdate()
    {
        dieMode = 0;
        kinematicsUpdate();
        // 每更新2次掉1血
        if (isLostHealth) AddHealth(-1);
        isLostHealth = !isLostHealth;
    }

    private void DieUpdate()
    {
        
    }

    private void DeathSound()
    {
        if (dieMode == 0)
        {
            int idx = Random.Range(0, ResourceConfig.sound_zombiedie_fallings.Length);
            AudioManager.Instance.playClip(ResourceConfig.sound_zombiedie_fallings[idx]);
        }
    }

    private void onDeathAnimComplete()
    {
        ZombieManager.Instance.removeZombie(this);
        Destroy(gameObject);
        Destroy(zombieHead);
    }

    private void kill(int dieMode=0)
    {
        this.dieMode = dieMode;
        setHealthState(ZombieHealthState.Die);
    }
}
