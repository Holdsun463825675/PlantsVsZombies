using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ZombieID
{
    None = 0,
    NormalZombie = 1,
    FlagZombie = 2,
    ConeHeadZombie = 3,
    BucketZombie = 5,
    ScreenDoorZombie = 7,
    FootballZombie = 8,
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

public enum ZombieUnderAttackSound
{
    Splat,
    Plastic,
    Shield,
}

public class Zombie : MonoBehaviour, IClickable
{
    // 需继承实现
    public ZombieID zombieID;
    protected float baseSpeed;
    protected float speed;
    public float speedRatio;
    private float speedChangeDuration;

    protected int maxHealth, currHealth;
    protected int maxArmor1Health, currArmor1Health;
    protected int maxArmor2Health, currArmor2Health;
    protected float lostArmHealthPercentage, lostHeadPercentage, dieHealthPercentage;

    protected int attackPoint;
    protected bool isLostHealth;
    public float spawnWeight;
    public GameObject zombieHeadPrefab;
    private GameObject zombieHead;

    private float groanTime, groanTimer;
    private float healthLossTime, healthLossTimer; // 掉头后多久掉1滴血
    private float HealthPercentage;
    private int dieMode;
    protected float speedLevel;
    private Transform losingGame;

    private ZombieMoveState moveState;
    private ZombieHealthState healthState;

    public ZombieUnderAttackSound underAttackSound;
    public int underAttackSoundPriority;

    private TextMeshPro HPText;
    private Transform lostHeadPlace;
    private List<Plant> targets;
    private Animator lostHeadAnim;

    private Animator anim;
    private Collider2D c2d;
    private Collider2D Armor2_c2d;

    private Tween currentMoveTween;

    protected virtual void Awake()
    {
        zombieID = ZombieID.None;
        baseSpeed = 0.2f;
        speed = Random.Range(1.0f, 2.0f) * baseSpeed;
        //speedRatio = 1.0f;
        //speedChangeDuration = 0.0f;
        speedRatio = 0.5f;
        speedChangeDuration = 999.0f;

        maxHealth = 270; currHealth = maxHealth;
        maxArmor1Health = 0; currArmor1Health = maxArmor1Health;
        maxArmor2Health = 0; currArmor2Health = maxArmor2Health;
        lostArmHealthPercentage = 0.666f; lostHeadPercentage = 0.333f; dieHealthPercentage = 1e-10f;

        attackPoint = 50;
        isLostHealth = false;
        spawnWeight = 1.0f;

        speedLevel = (speed - baseSpeed) / baseSpeed;
        HealthPercentage = 1.0f;
        groanTime = 24.0f; groanTimer = 19.0f + Random.Range(0.0f, 2.0f);
        healthLossTime = 0.05f; healthLossTimer = 0.0f;
        dieMode = 0;
        moveState = ZombieMoveState.Stop;

        underAttackSound = ZombieUnderAttackSound.Splat;
        underAttackSoundPriority = 1;

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
        child = transform.Find("Armor2");
        if (child) Armor2_c2d = child.GetComponent<Collider2D>();
        if (Armor2_c2d) Armor2_c2d.enabled = false;
    }

    private void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = true;
    }

    protected virtual void Update()
    {
        HPText.text = $"HP: {currHealth}/{maxHealth}";
        if (currArmor1Health > 0) HPText.text = $"A1: {currArmor1Health}/{maxArmor1Health}\n" + HPText.text;
        if (currArmor2Health > 0) HPText.text = $"A2: {currArmor2Health}/{maxArmor2Health}\n" + HPText.text;
        if (Armor2_c2d) Armor2_c2d.enabled = currArmor2Health > 0;

        HPText.gameObject.SetActive(SettingSystem.Instance.settingsData.zombieHealth);
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        HealthPercentage = (float)currHealth / (float)maxHealth;

        anim.SetFloat(AnimatorConfig.zombie_healthPercentage, HealthPercentage);

        anim.SetFloat(AnimatorConfig.zombie_armor1HealthPercentage, maxArmor1Health == 0.0f ? 0.0f : (float)currArmor1Health / (float)maxArmor1Health);
        anim.SetFloat(AnimatorConfig.zombie_armor2HealthPercentage, maxArmor2Health == 0.0f ? 0.0f : (float)currArmor2Health / (float)maxArmor2Health);

        anim.SetFloat(AnimatorConfig.zombie_speedLevel, speedLevel);
        // 减速比例
        anim.SetFloat(AnimatorConfig.zombie_speedRatio, speedRatio);
        if (lostHeadAnim) lostHeadAnim.SetFloat(AnimatorConfig.zombie_speedRatio, speedRatio);
        if (currentMoveTween != null) currentMoveTween.timeScale = speedRatio;

        if (HealthPercentage >= lostArmHealthPercentage) setHealthState(ZombieHealthState.Healthy);
        if (HealthPercentage >= lostHeadPercentage && HealthPercentage < lostArmHealthPercentage) setHealthState(ZombieHealthState.LostArm);
        if (HealthPercentage >= dieHealthPercentage && HealthPercentage < lostHeadPercentage) setHealthState(ZombieHealthState.LostHead);
        if (HealthPercentage < dieHealthPercentage) setHealthState(ZombieHealthState.Die);

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
        if (currArmor2Health > 0 && Armor2_c2d) Armor2_c2d.enabled = true;
        anim.SetBool(AnimatorConfig.zombie_game, true);
        losingGame = MapManager.Instance.currMap.endlinePositions[0];
        Vector3 target = new Vector3(losingGame.position.x, transform.position.y, transform.position.z);
        currentMoveTween = transform.DOMove(target, Vector3.Distance(target, transform.position) / speed)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                if (healthState == ZombieHealthState.Healthy || healthState == ZombieHealthState.LostArm) GameManager.Instance.setState(GameState.Losing);
            });
    }

    // 暂停继续功能
    public void Pause()
    {
        if (moveState == ZombieMoveState.Move) transform.DOPause();
        anim.enabled = false;
        if ((healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) && lostHeadAnim) lostHeadAnim.enabled = false;
    }

    public void Continue()
    {
        if (moveState == ZombieMoveState.Move) transform.DOPlay();
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
        return maxHealth + maxArmor1Health + maxArmor2Health;
    }

    public int getCurrHealth()
    {
        return currHealth + currArmor1Health + currArmor2Health;
    }

    public ZombieHealthState getHealthState()
    {
        return healthState; 
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
        moveState = state;
        if (state == ZombieMoveState.Move) transform.DOPlay();
        if (state == ZombieMoveState.Stop) transform.DOPause();
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
            // 掉头动画
            lostHeadAnim = zombieHead.GetComponent<Animator>();
            AudioManager.Instance.playClip(ResourceConfig.sound_zombiedie_limbsPop);
        }
        if (state == ZombieHealthState.Die)
        {
            if (dieMode == 0 && !lostHeadAnim) // 掉头动画
            {
                lostHeadAnim = zombieHead.GetComponent<Animator>();
                AudioManager.Instance.playClip(ResourceConfig.sound_zombiedie_limbsPop);
            }
            currArmor2Health = 0;
            transform.DOKill();
            c2d.enabled = false;
            if (Armor2_c2d) Armor2_c2d.enabled = false;
            anim.SetInteger(AnimatorConfig.zombie_dieMode, dieMode);
        }
    }

    private void kinematicsUpdate()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false) return;
        Plant target = getAttackTarget();
        if (target)
        {
            setMoveState(ZombieMoveState.Stop);
            anim.SetBool(AnimatorConfig.zombie_isAttack, true);
            return;
        }
        anim.SetBool(AnimatorConfig.zombie_isAttack, false);
    }

    private void groanUpdate()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false || healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) return;
        // 僵尸叫声
        groanTimer += Time.deltaTime;
        if (groanTimer >= groanTime)
        {
            int idx = Random.Range(0, ResourceConfig.sound_other_groans.Length);
            AudioManager.Instance.playClip(ResourceConfig.sound_other_groans[idx]);
            groanTimer = 0.0f;
        }
    }

    private void speedChangeUpdate()
    {
        if (speedChangeDuration <= 0.0f)
        {
            speedRatio = 1.0f;
            return;
        }
        speedChangeDuration -= Time.deltaTime;
    }

    public void AddCurrHealth(int point)
    {
        currHealth += point;
        if (currHealth > maxHealth) currHealth = maxHealth;
        if (currHealth <= 0) currHealth = 0;
    }

    public int AddArmor1Health(int point) // 返回溢出的扣血伤害
    {
        currArmor1Health += point;
        if (currArmor1Health > maxArmor1Health) currArmor1Health = maxArmor1Health;
        if (currArmor1Health < 0)
        {
            int res = -currArmor1Health;
            currArmor1Health = 0;
            return res;
        }
        return 0;
    }

    public void AddArmor2Health(int point)
    {
        currArmor2Health += point;
        if (currArmor2Health > maxArmor2Health) currArmor2Health = maxArmor2Health;
        if (currArmor2Health < 0) currArmor2Health = 0;
    }

    public void UnderAttack(int point, int mode=0)
    {
        dieMode = mode;
        anim.SetBool(AnimatorConfig.zombie_underAttack, true);
        int hurtPoint = (int)((float)point * SettingSystem.Instance.settingsData.hurtRate); // 根据受伤比例计算伤害

        if (currArmor1Health > 0) AddCurrHealth(-AddArmor1Health(-hurtPoint));
        else AddCurrHealth(-hurtPoint);
    }

    public void setSpeedRatio(float speedRatio, float speedChangeDuration=12.0f)
    {
        this.speedChangeDuration = speedChangeDuration;
        if (speedRatio < this.speedRatio) AudioManager.Instance.playClip(ResourceConfig.sound_zombie_frozen);
        this.speedRatio = speedRatio;
    }

    private Plant getAttackTarget()
    {
        foreach (Plant plant in targets) if (plant.type == PlantType.Surrounding) return plant;
        foreach (Plant plant in targets) if (plant.type == PlantType.Normal) return plant;
        foreach (Plant plant in targets) if (plant.type == PlantType.Carrier) return plant;
        return null; // 不吃飞行类植物
    }

    protected virtual void Attack()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false || healthState == ZombieHealthState.LostHead || healthState == ZombieHealthState.Die) return;
        Plant target = getAttackTarget();
        if (target) target.UnderAttack(attackPoint);
    }

    private void HealthyUpdate()
    {
        kinematicsUpdate();
        groanUpdate();
        speedChangeUpdate();
    }

    private void LostArmUpdate()
    {
        kinematicsUpdate();
        groanUpdate();
        speedChangeUpdate();
    }

    private void LostHeadUpdate()
    {
        dieMode = 0;
        kinematicsUpdate();
        speedChangeUpdate();

        if (Time.timeScale <= 0) return; // 暂停时不掉血

        healthLossTimer += Time.deltaTime;
        // TODO: 玄学
        if (healthLossTimer >= healthLossTime / Time.timeScale)
        {
            //AddHealth(-(int)(healthLossTimer / healthLossTime));
            //healthLossTimer = healthLossTimer % healthLossTime;
            AddCurrHealth(-1);
            healthLossTimer = 0.0f;
        }
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

    public void kill(int dieMode=0)
    {
        this.dieMode = dieMode;
        currArmor2Health = 0;
        currArmor1Health = 0;
        currHealth = 0;
    }

    // 父物体处理触发事件的方法
    public virtual void OnChildTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void OnChildTriggerExit2D(Collider2D collision)
    {

    }
}
