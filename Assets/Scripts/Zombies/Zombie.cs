using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ZombieID
{
    None = 0,
    NormalZombie = 1, FlagZombie = 2, ConeHeadZombie = 3, PoleVaultingZombie = 4, BucketZombie = 5,
    NewspaperZombie = 6, ScreenDoorZombie = 7, FootballZombie = 8,
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
    public float deceleration; // 1为无减速，0为冰冻
    private float decelerationDuration, frozenDuration, butterDuration; // 减速、冰冻、黄油持续时间
    public bool temptation;

    protected int maxHealth, currHealth;
    protected int maxArmor1Health, currArmor1Health;
    protected int maxArmor2Health, currArmor2Health;
    protected float lostArmHealthPercentage, lostHeadPercentage, dieHealthPercentage;

    protected int attackPoint;
    protected List<int> targetRows; // 可攻击的行，0为任意，大于0为行数
    protected List<int> effectRows; // 可起作用的行，0为任意，大于0为行数
    protected bool isLostHealth;
    public float spawnWeight;
    protected GameObject zombieHeadPrefab;
    private List<GameObject> Debuff;
    private GameObject zombieHead;

    private float groanTime, groanTimer;
    private float healthLossTime, healthLossTimer; // 掉头后多久掉1滴血
    private float HealthPercentage;
    private int dieMode;
    protected float speedLevel;
    protected Transform losingGame;
    protected GameObject shadow;

    public int row, col; // 所处行列，游戏模式下大于0
    public bool isPlantKill; // 是否能被植物机制杀，大嘴花、水草之类的
    public bool isBulletHit; // 是否能被子弹造成伤害

    protected ZombieMoveState moveState;
    protected ZombieHealthState healthState;

    public ZombieUnderAttackSound underAttackSound;
    public int underAttackSoundPriority;

    public Armor2 armor2;
    protected TextMeshPro HPText;
    private Transform lostHeadPlace;
    private List<Plant> targetPlants;
    private List<Zombie> targetZombies;
    private Animator lostHeadAnim;
    protected List<Plant> effectTargetPlants, effectBowlingTargetPlants;
    protected List<Zombie> effectTargetZombies;

    protected Animator anim;
    private Collider2D c2d, bowling_c2d;
    private Collider2D Armor2_c2d, Armor2_bowling_c2d;
    protected Collider2D effect_c2d, effect_bowling_c2d;

    protected Tween currentMoveTween;

    protected virtual void Awake()
    {
        zombieID = ZombieID.None;
        setMoveSpeed();
        deceleration = 1.0f;
        decelerationDuration = 0.0f; frozenDuration = 0.0f; butterDuration = 0.0f;
        temptation = false;

        maxHealth = 270; currHealth = maxHealth;
        maxArmor1Health = 0; currArmor1Health = maxArmor1Health;
        maxArmor2Health = 0; currArmor2Health = maxArmor2Health;
        lostArmHealthPercentage = 0.666f; lostHeadPercentage = 0.333f; dieHealthPercentage = 1e-10f;

        attackPoint = 50;
        targetRows = new List<int>();
        isLostHealth = false;
        spawnWeight = 1.0f;

        HealthPercentage = 1.0f;
        groanTime = 24.0f; groanTimer = 19.0f + Random.Range(0.0f, 2.0f);
        healthLossTime = 0.05f; healthLossTimer = 0.0f;
        dieMode = 0;
        moveState = ZombieMoveState.Stop;

        underAttackSound = ZombieUnderAttackSound.Splat;
        underAttackSoundPriority = 1;

        row = 0; col = 0;
        isPlantKill = true;
        isBulletHit = true;

        zombieHeadPrefab = PrefabSystem.Instance.others[0];
        targetPlants = new List<Plant>(); targetZombies = new List<Zombie>();
        effectTargetPlants = new List<Plant>(); effectBowlingTargetPlants = new List<Plant>(); effectTargetZombies = new List<Zombie>();
        anim = GetComponent<Animator>();
        anim.SetBool(AnimatorConfig.zombie_game, false);
        c2d = GetComponent<Collider2D>();
        c2d.enabled = false;
        bowling_c2d = transform.Find("Bowling").GetComponent<Collider2D>();
        bowling_c2d.enabled = false;
        armor2 = transform.Find("Armor2").GetComponent<Armor2>();
        shadow = transform.Find("Shadow").gameObject; shadow.SetActive(true);
        Transform child = transform.Find("HPText");
        if (child) HPText = child.GetComponent<TextMeshPro>();
        if (HPText) HPText.gameObject.SetActive(true);
        child = transform.Find("LostHeadPlace");
        if (child) lostHeadPlace = child.GetComponent<Transform>();
        child = transform.Find("Armor2");
        if (child)
        {
            Armor2_c2d = child.GetComponent<Collider2D>();
            if (Armor2_c2d) Armor2_c2d.enabled = false;
            Armor2_bowling_c2d = child.Find("Armor2_Bowling").GetComponent<Collider2D>();
            if (Armor2_bowling_c2d) Armor2_bowling_c2d.enabled = false;
        }
        child = transform.Find("EffectPlace");
        if (child)
        {
            effect_c2d = child.GetComponent<Collider2D>();
            effect_c2d.enabled = false;
            effect_c2d.GetComponent<TriggerForwarder>().SetZombieParentHandler(this);
        }
        child = transform.Find("EffectPlace_Bowling");
        if (child)
        {
            effect_bowling_c2d = child.GetComponent<Collider2D>();
            effect_bowling_c2d.enabled = false;
            effect_bowling_c2d.GetComponent<TriggerForwarder>().SetZombieParentHandler(this);
        }
        Debuff = new List<GameObject>();
        Debuff.Add(transform.Find("Debuff/Frozen").gameObject);
        Debuff.Add(transform.Find("Debuff/Butter").gameObject);
        foreach (GameObject debuff in Debuff) debuff.SetActive(false);
    }

    private void Start()
    {
        ClickPriority priority = gameObject.AddComponent<ClickPriority>();
        priority.priority = 1;
        priority.isClickable = true;

        HPChanged(); AddArmor1Health(0); AddArmor2Health(0);
    }

    protected virtual void Update()
    {
        HPTextActiveUpdate();
        if (GameManager.Instance.state == GameState.Paused || GameManager.Instance.state == GameState.Losing) return;

        DebuffUpdate();

        switch (healthState)
        {
            case ZombieHealthState.Healthy:
                HealthyAndLostArmUpdate(); break;
            case ZombieHealthState.LostArm:
                HealthyAndLostArmUpdate(); break;
            case ZombieHealthState.LostHead:
                LostHeadUpdate(); break;
            case ZombieHealthState.Die:
                DieUpdate(); break;
            default:
                break;
        }
    }

    public void setGameMode(int row, int col)
    {
        this.row = row; this.col = col;
        targetRows = new List<int> { this.row }; // 默认只能攻击本行
        effectRows = new List<int> { this.row };
        c2d.enabled = true; bowling_c2d.enabled = true;
        if (currArmor2Health > 0)
        {
            if (Armor2_c2d) Armor2_c2d.enabled = true;
            if (Armor2_bowling_c2d) Armor2_bowling_c2d.enabled = true;
        }
        if (effect_c2d) effect_c2d.enabled = true;
        if (effect_bowling_c2d) effect_bowling_c2d.enabled = true;
        anim.SetBool(AnimatorConfig.zombie_game, true);
        anim.SetFloat(AnimatorConfig.zombie_speedLevel, speedLevel);
        losingGame = MapManager.Instance.currMap.endlinePositions[0];
        moveToNextCell();
    }

    protected virtual void setMoveSpeed(float minSpeed = 0.2f, float maxSpeed = 0.4f)
    {
        if (minSpeed > maxSpeed) return;
        baseSpeed = maxSpeed - minSpeed;
        speed = Random.Range(minSpeed, maxSpeed);
        speedLevel = baseSpeed == 0 ? 1 : (speed - minSpeed) / baseSpeed;
    }

    // 暂停继续功能
    public virtual void Pause()
    {
        if (moveState == ZombieMoveState.Move) transform.DOPause();
        anim.enabled = false;
        if (lostHeadAnim) lostHeadAnim.enabled = false;
    }

    public virtual void Continue()
    {
        if (moveState == ZombieMoveState.Move) transform.DOPlay();
        anim.enabled = true;
        if (lostHeadAnim) lostHeadAnim.enabled = true;
    }

    public void OnClick()
    {

    }

    public int getMaxHealth()
    {
        return maxHealth + maxArmor1Health + maxArmor2Health;
    }

    public int getCurrHealth()
    {
        return currHealth + currArmor1Health + currArmor2Health;
    }

    public bool isHealthy()
    {
        return (healthState == ZombieHealthState.Healthy || healthState == ZombieHealthState.LostArm) && !temptation; 
    }

    public bool canAct() // 是否可行动，运动、啃食、特殊状态转换等
    {
        return frozenDuration <= 0 && butterDuration <= 0;
    }

    public int setSortingOrder(int sortingOrder)
    {
        int count = 0;
        this.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + count++;
        foreach (GameObject debuff in Debuff) debuff.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + count++;
        if (HPText) HPText.sortingOrder = sortingOrder + count++;
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
            relieveFrozen(); relieveButter(); // 消除减速和黄油
            setLostHeadAnim();
        }
        if (state == ZombieHealthState.Die)
        {
            if (dieMode == 0 && !lostHeadAnim) setLostHeadAnim();
            AddArmor2Health(-currArmor2Health);
            transform.DOKill();
            c2d.enabled = false;
            bowling_c2d.enabled = false;
            if (Armor2_c2d) Armor2_c2d.enabled = false;
            if (Armor2_bowling_c2d) Armor2_bowling_c2d.enabled = false;
            if (effect_c2d) effect_c2d.enabled = false;
            if (effect_bowling_c2d) effect_bowling_c2d.enabled = false;
            relieveFrozen(); relieveButter(); // 消除减速和黄油
            if (dieMode == 1 || dieMode == 3) relieveDeceleration(); // 消除减速效果
            anim.SetInteger(AnimatorConfig.zombie_dieMode, dieMode);
        }
    }

    protected void moveToNextCell()
    {
        Vector3 target = new Vector3(losingGame.position.x, transform.position.y, transform.position.z);
        if (temptation) target.x = MapManager.Instance.currMap.endlinePositions[1].position.x;
        Cell targetCell = CellManager.Instance.getCell(row, temptation ? col + 1 : col - 1);
        if (targetCell == null)
        {
            currentMoveTween = transform.DOMove(target, speed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
            {
                if (isHealthy()) GameManager.Instance.setState(GameState.Losing);
                else kill(dieMode: 2);
            });
        }
        else
        {
            target = targetCell.transform.position;
            currentMoveTween = transform.DOMove(target, speed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(() =>
            {
                if (temptation) col += 1;
                else col -= 1;
                moveToNextCell();
            });
        }
    }

    protected virtual void HPTextActiveUpdate()
    {
        HPText.gameObject.SetActive(SettingSystem.Instance.settingsData.zombieHealth);
    }

    protected virtual void kinematicsUpdate()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false) return;
        Zombie zombie = getAttackTargetZombie();
        Plant target = getAttackTargetPlant();
        if ((zombie || target) && canAct())
        {
            setMoveState(ZombieMoveState.Stop);
            anim.SetBool(AnimatorConfig.zombie_isAttack, true);
            return;
        }
        if (canAct()) anim.SetBool(AnimatorConfig.zombie_isAttack, false);
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

    private void DebuffUpdate() // 异常状态更新：减速、冰冻、黄油
    {
        decelerationDuration -= Time.deltaTime;
        frozenDuration -= Time.deltaTime;
        butterDuration -= Time.deltaTime;
        if (decelerationDuration < 0) // 减速
        {
            deceleration = 1.0f;
            decelerationDuration = 0;
        }

        if (frozenDuration < 0) frozenDuration = 0; // 冰冻
        Debuff[0].SetActive(frozenDuration > 0);

        if (butterDuration < 0) butterDuration = 0; // 黄油
        Debuff[1].SetActive(butterDuration > 0);

        float speedRatio = deceleration;
        if (frozenDuration > 0 || butterDuration > 0) speedRatio = 0;
        anim.SetFloat(AnimatorConfig.zombie_speedRatio, speedRatio);
        anim.SetBool(AnimatorConfig.zombie_deceleration, deceleration < 1.0f || frozenDuration > 0);
        anim.SetBool(AnimatorConfig.zombie_temptation, temptation);

        if (currentMoveTween != null)
        {
            if (frozenDuration > 0 || butterDuration > 0) speedRatio = 0;
            currentMoveTween.timeScale = speedRatio;
        }

        if (lostHeadAnim)
        {
            lostHeadAnim.SetFloat(AnimatorConfig.zombie_speedRatio, deceleration);
            lostHeadAnim.SetBool(AnimatorConfig.zombie_temptation, temptation);
        }
    }

    protected void HPChanged()
    {
        HPText.text = $"HP: {currHealth}/{maxHealth}";
        if (currArmor1Health > 0) HPText.text = $"A1: {currArmor1Health}/{maxArmor1Health}\n" + HPText.text;
        if (currArmor2Health > 0) HPText.text = $"A2: {currArmor2Health}/{maxArmor2Health}\n" + HPText.text;
        if (Armor2_c2d) Armor2_c2d.enabled = currArmor2Health > 0;
        if (Armor2_bowling_c2d) Armor2_bowling_c2d.enabled = currArmor2Health > 0;

        HealthPercentage = (float)currHealth / (float)maxHealth;
        if (HealthPercentage >= lostArmHealthPercentage) setHealthState(ZombieHealthState.Healthy);
        if (HealthPercentage >= lostHeadPercentage && HealthPercentage < lostArmHealthPercentage) setHealthState(ZombieHealthState.LostArm);
        if (HealthPercentage >= dieHealthPercentage && HealthPercentage < lostHeadPercentage) setHealthState(ZombieHealthState.LostHead);
        if (HealthPercentage < dieHealthPercentage) setHealthState(ZombieHealthState.Die);

        anim.SetFloat(AnimatorConfig.zombie_healthPercentage, HealthPercentage);
    }

    public void AddCurrHealth(int point)
    {
        currHealth += point;
        if (currHealth > maxHealth) currHealth = maxHealth;
        if (currHealth <= 0) currHealth = 0;

        HPChanged();
    }

    public int AddArmor1Health(int point) // 返回溢出的扣血伤害
    {
        int res = 0;
        currArmor1Health += point;
        if (currArmor1Health > maxArmor1Health) currArmor1Health = maxArmor1Health;
        if (currArmor1Health < 0)
        {
            res = -currArmor1Health;
            currArmor1Health = 0;
        }
        if (currArmor1Health == 0)
        {
            underAttackSound = ZombieUnderAttackSound.Splat;
            underAttackSoundPriority = 1;
        } 
        anim.SetFloat(AnimatorConfig.zombie_armor1HealthPercentage, maxArmor1Health == 0.0f ? 0.0f : (float)currArmor1Health / (float)maxArmor1Health);
        HPChanged();
        return res;
    }

    public virtual void AddArmor2Health(int point)
    {
        currArmor2Health += point;
        if (currArmor2Health > maxArmor2Health) currArmor2Health = maxArmor2Health;
        if (currArmor2Health < 0) currArmor2Health = 0;
        anim.SetFloat(AnimatorConfig.zombie_armor2HealthPercentage, maxArmor2Health == 0.0f ? 0.0f : (float)currArmor2Health / (float)maxArmor2Health);
        HPChanged();
    }

    public void UnderAttack(int point, int mode=0)
    {
        dieMode = mode;
        anim.SetBool(AnimatorConfig.zombie_underAttack, true);
        int hurtPoint = point;
        if (!temptation) hurtPoint = (int)((float)point * SettingSystem.Instance.settingsData.hurtRate); // 不魅惑的根据受伤比例计算伤害

        if (currArmor1Health > 0) AddCurrHealth(-AddArmor1Health(-hurtPoint));
        else AddCurrHealth(-hurtPoint);
    }

    public void setDeceleration(float deceleration=0.5f, float decelerationDuration=15.0f)
    {
        if (deceleration > this.deceleration) return; // 弱于当前减速则无效
        this.decelerationDuration = decelerationDuration;
        if (deceleration < this.deceleration)
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_zombie_frozen);
            this.deceleration = deceleration;
        }
    }

    public virtual void setFrozen(float frozenDuration=5.0f)
    {
        setDeceleration(); // 冰冻通常附带减速
        if (!isHealthy() && !temptation) return; // 冰冻只对健康状态有效
        if (this.frozenDuration <= frozenDuration) this.frozenDuration = frozenDuration;
    }

    public void setButter(float butterDuration=5.0f)
    {
        if (!isHealthy() && !temptation) return; // 只对健康状态有效
        if (this.butterDuration <= butterDuration) this.butterDuration = butterDuration;
    }

    public void setTemptation()
    {
        if (temptation) return;
        AudioManager.Instance.playClip(ResourceConfig.sound_plant_mindcontrolled);
        temptation = true;

        // 图层改为植物
        gameObject.layer = LayerSystem.Plant_layer;
        Destroy(GetComponent<Rigidbody2D>()); // 移除刚体

        // 解除所有debuff
        relieveFrozen(); relieveDeceleration(); relieveButter();

        // 递归修改所有子物体
        Transform[] children = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in children) child.gameObject.layer = gameObject.layer;

        transform.DOKill(); currentMoveTween = null;
        moveToNextCell();
    }

    public void relieveDeceleration()
    {
        deceleration = 1.0f;
        decelerationDuration = 0;
    }

    public void relieveFrozen()
    {
        frozenDuration = 0;
    }

    public void relieveButter()
    {
        butterDuration = 0;
    }

    private void setLostHeadAnim()
    {
        zombieHead = GameObject.Instantiate(zombieHeadPrefab, lostHeadPlace.position, Quaternion.identity);
        zombieHead.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder + 1;
        lostHeadAnim = zombieHead.GetComponent<Animator>();
        AudioManager.Instance.playClip(ResourceConfig.sound_zombiedie_limbsPop);
    }

    protected virtual bool CanAttack(Plant plant)
    {
        return (plant.row == 0 || targetRows.Contains(0) || targetRows.Contains(plant.row)) && plant.type != PlantType.Flight;
    }

    protected virtual bool CanAttack(Zombie zombie)
    {
        return (zombie.row == 0 || targetRows.Contains(0) || targetRows.Contains(zombie.row)) && zombie.isBulletHit;
    }

    protected Plant getAttackTargetPlant()
    {
        foreach (Plant plant in targetPlants) if (plant.type == PlantType.Surrounding && CanAttack(plant)) return plant;
        foreach (Plant plant in targetPlants) if (plant.type == PlantType.Normal && CanAttack(plant)) return plant;
        foreach (Plant plant in targetPlants) if (plant.type == PlantType.Carrier && CanAttack(plant)) return plant;
        return null; // 不吃飞行类植物
    }

    protected Zombie getAttackTargetZombie()
    {
        foreach (Zombie zombie in targetZombies) if (zombie && CanAttack(zombie)) return zombie;
        return null;
    }

    protected virtual void Attack()
    {
        if (anim.GetBool(AnimatorConfig.zombie_game) == false || (!isHealthy() && !temptation)) return;

        Zombie zombie = getAttackTargetZombie(); // 先攻击僵尸
        if (zombie)
        {
            AudioManager.Instance.playClip(ResourceConfig.sound_zombieeat_chomps[Random.Range(0, ResourceConfig.sound_zombieeat_chomps.Length)]);
            zombie.UnderAttack(attackPoint);
            return;
        }

        Plant target = getAttackTargetPlant();
        if (target) target.UnderAttack(attackPoint, zombie: this);
    }

    protected virtual bool CanEffect(Plant plant)
    {
        return (plant.row == 0 || effectRows.Contains(0) || effectRows.Contains(plant.row)) && plant.type != PlantType.Flight;
    }

    protected virtual bool CanEffect(Zombie zombie)
    {
        return (zombie.row == 0 || effectRows.Contains(0) || effectRows.Contains(zombie.row)) && zombie.isHealthy();
    }

    protected virtual bool HaveEffectTargetPlant()
    {
        foreach (Plant plant in effectTargetPlants) if (plant && CanEffect(plant)) return true;
        foreach (Plant plant in effectBowlingTargetPlants) if (plant && CanEffect(plant)) return true;
        return false;
    }

    protected virtual bool HaveEffectTargetZombie()
    {
        foreach (Zombie zombie in effectTargetZombies) if (zombie && CanEffect(zombie)) return true;
        return false;
    }

    protected virtual void HealthyAndLostArmUpdate()
    {
        kinematicsUpdate();
        groanUpdate();
    }

    protected virtual void LostHeadUpdate()
    {
        dieMode = 0;
        kinematicsUpdate();

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

    protected virtual void DieUpdate()
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
        AddArmor2Health(-currArmor2Health);
        AddArmor1Health(-currArmor1Health);
        AddCurrHealth(-currHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.plant:
                Plant target = collision.GetComponent<Plant>();
                if (target && !targetPlants.Contains(target)) targetPlants.Add(target);
                break;
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie && !targetZombies.Contains(zombie)) targetZombies.Add(zombie);
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.plant:
                Plant target = collision.GetComponent<Plant>();
                if (target) targetPlants.Remove(target);
                break;
            case TagConfig.zombie:
                Zombie zombie = collision.GetComponent<Zombie>();
                if (zombie) targetZombies.Remove(zombie);
                break;
            default:
                break;
        }
    }

    // 父物体处理触发事件的方法
    public virtual void OnChildTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.plant:
                Plant effectTarget = collision.GetComponent<Plant>();
                if (effectTarget && !effectTargetPlants.Contains(effectTarget)) effectTargetPlants.Add(effectTarget);
                break;
            case TagConfig.bowling_plant:
                Plant effectBowlingTarget = collision.GetComponent<Plant>();
                if (effectBowlingTarget && !effectBowlingTargetPlants.Contains(effectBowlingTarget)) effectBowlingTargetPlants.Add(effectBowlingTarget);
                break;
            case TagConfig.zombie:
                Zombie zombie = GetComponent<Zombie>();
                if (zombie && !effectTargetZombies.Contains(zombie)) effectTargetZombies.Add(zombie);
                break;
            default:
                break;
        }
    }

    public virtual void OnChildTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case TagConfig.plant:
                Plant effectTarget = collision.GetComponent<Plant>();
                if (effectTarget) effectTargetPlants.Remove(effectTarget);
                break;
            case TagConfig.bowling_plant:
                Plant effectBowlingTarget = collision.GetComponent<Plant>();
                if (effectBowlingTarget) effectBowlingTargetPlants.Remove(effectBowlingTarget);
                break;
            case TagConfig.zombie:
                Zombie zombie = GetComponent<Zombie>();
                if (zombie) effectTargetZombies.Remove(zombie);
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        if (currentMoveTween != null) currentMoveTween.Kill();
    }
}
