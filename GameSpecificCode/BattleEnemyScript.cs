using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BattleEnemyScript : MonoBehaviour {

    /* enemy health if dead or not
     * will take care of animations, particle system
     * Drops chossen from a list of items
     */
    
    //Handlers
    public int health = 1;
    public int originalHealth;
    public int damageTaken = 1;
    public BoxCollider HitBox;

    //Script References
    public RewardPool rewardPool;
    public DoDamageToPlayer doDamageScript;
    public BarController barScript;
    public GameObject CameraShake;
    public GameObject flash;
    public float flashTime;
    public float smooth;

    //Particle Effects Reference and variables
    public GameObject posionEffect;
    public GameObject burningEffect;
    public GameObject deathEffect;

    //Boss Stuff
    public GameObject BossHealthObject;
    public Slider BossHealthBar;
    public Text BossText;
    public string bossName;

    //Sound stuff
    public DOTweenAnimation deathAnim;
    public AudioClip soundEffects;
    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioSource audioS;

    //Flags go here
    public  bool _isBoss = false;
    private bool _isPoisoned = false;
    private bool _isBurning = false;
    private bool _isDead = false;

    // Use this for initialization
    void Start ()
    {
        #region Get all refernces
        if(_isBoss) {
            BossHealthObject = GameObject.Find("BossHealthBar");
            BossHealthBar = BossHealthObject.GetComponent<Slider>();
            BossText = GameObject.Find("BossNameText").GetComponent<Text>();
        }
        HitBox = GameObject.Find("HitArea").GetComponent<BoxCollider>();
        barScript = GameObject.Find("Captain").GetComponent<BarController>();
        audioS = GameObject.Find("SFX").GetComponent<AudioSource>();
        CameraShake = GameObject.Find("CameraShaker");
        rewardPool = GameObject.Find("Reward Pool").GetComponent<RewardPool>();
        #endregion
        StartCoroutine(InitCoroutine());
        flash.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        damageTaken = barScript.damageDone;

        #region Check if Dead
        if (health <= 0)
        {
            if (!_isBoss)
            {
                doDamageScript.enabled = false; 
            }
            if (_isDead == false)
            {
                if (!_isBoss)
                {
                    barScript.SetScore(barScript.GetScore() + Random.Range(2, 7));
                    deathAnim.DOPlayById("DEATH");
                    float pickReward = Random.value;
                    if (pickReward >= 0.1f)
                    {
                        Loot(0);

                        if(barScript.GetGoldTooth() == true){
                            Loot(0);
                        }
                    }
                    else
                    {
                        Loot(1);
                    }
                }
                else {
                    barScript.SetScore(barScript.GetScore() + 10);
                    Loot(2);
                }
                
                //If boss do instant death timer else apply 2 seconds of death time
                if(_isBoss) {
                    StartCoroutine("DeathTimer", 0f);
                } else {
                    StartCoroutine("DeathTimer", 1.5f);
                }
                //audioS.PlayOneShot(deathSound, 1.0F);
                _isDead = true;
            }
        }
        #endregion

        #region Flash when enemy is hit
        if (flashTime > 0)
        {
            flashTime -= Time.deltaTime;
            flash.SetActive(true);
        }
        else if (flashTime <= 0)
        {
            flashTime = 0;
            flash.SetActive(false);
        }
        #endregion

    }

    public void DoDamage()
    {
        //Iron Dagger
        int extraDamage = 0;
        if (barScript.GetIronDagger() == true){
            extraDamage = (int)Mathf.Ceil(damageTaken * 0.1f);
            Debug.Log("Doing Extra Damage: " + extraDamage);
        }

        //Sword of hate
        if (barScript.GetSwordOfHate() == true){
            float fireDamageChance = Random.value;
            if(Random.value <= .25f){
                StartCoroutine(BurningEffect(5,1));
            }
        }

        //Scythe
        int extraDamageScythe = 0;
        if (barScript.GetScythe() == true){
            extraDamageScythe = (int)Mathf.Ceil(damageTaken * 0.1f);
            Debug.Log("Doing Extra Damage: " + extraDamageScythe);
        }
        
        //Cursed Blade
        int cursedBladeDamage = 0;
        if (barScript.GetCursedBlade() == true){
            cursedBladeDamage = (int)Mathf.Ceil(damageTaken * 0.1f);
            float randomChance = Random.value;
            if(randomChance >= 0.95f){
                GameObject newCursedBladeObject = Instantiate(barScript.cursedBladeObject, this.gameObject.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
                newCursedBladeObject.transform.position = new Vector3(this.gameObject.transform.position.x + 0, 0.5f, this.gameObject.transform.position.z + 0);
            }
            Debug.Log("Doing Extra Damage: " + cursedBladeDamage);
        }

        //Sword of hate
        if (barScript.GetNightblade() == true){
            if(Random.value <= .25f){
                StartCoroutine(PoisonEffect(5,1));
            }
        }

        //Bloodthirster
        if (barScript.GetBloodthirster() == true){
            if(Random.value <= .55f){
                float lifeSteal = (damageTaken + extraDamage + extraDamageScythe + cursedBladeDamage) * 0.2f;
                barScript.HealthBar.value += lifeSteal;
            }
        }

        if(_isBoss != true)
        {
            this.gameObject.transform.Translate(Vector3.back * smooth * Time.deltaTime);
        }
        CameraShake.GetComponent<ScreenShake>().Shake(0.1f, 0.3f);
        health -= damageTaken + extraDamage + extraDamageScythe + cursedBladeDamage; 
        HitBox.enabled = false; 
        flashTime = 0.1f;
        audioS.PlayOneShot(soundEffects, 1.0F);
        audioS.PlayOneShot(deathSound, 1.0F);

        if (_isBoss == true)
        {
            if (BossHealthObject == null)
            {
                BossHealthObject.SetActive(true);
                BossText.text = bossName;
                BossHealthBar.maxValue = originalHealth;
                BossHealthBar.value = health;
            }
            else
            {
                BossHealthObject.SetActive(true);
                BossText.text = bossName;
                BossHealthBar.maxValue = originalHealth;
                BossHealthBar.value = health;
            }
        }
        else
        {   
            if (BossHealthBar == null){
                GameObject.Find("BossHealthObject");
                if (BossHealthBar != null){
                    BossHealthObject.SetActive(false);
                }
            }
            else {
                BossHealthObject.SetActive(false);
            }
        }
    }

    void Dead()
    {
        //Spawn object, Do animation/particles, Destroy Object
        if (!_isBoss)
        {
            doDamageScript.damageSet = false; 
        }
        flash.SetActive(false);

        if (_isBoss == true)
        {
            BossHealthObject.SetActive(false);
            barScript.challengeCounters[1]++;
            Instantiate(deathEffect, this.gameObject.transform.position, Quaternion.Euler(270, 45, 0));
            //Loot(2);
        }
        else
        {
            float pickReward = Random.value;
            if (pickReward <= 0.6)
            {
                //Loot(0);
            }
            else
            {
                //Loot(1);
            }
        }

        if(!_isBoss){
            if(barScript.GetAcidSkull() == true){
                float randomChance = Random.value;
                if(randomChance >= 0.9f){
                    barScript.AcidSkullTrigger();
                }
            }
        }

        barScript.LookForKey(2);
        barScript.killedEnemyCount++;
        barScript.challengeCounters[0]++;
        //Disable this GameObject instead of destroy
        Destroy(this.gameObject);
    }

    public IEnumerator DeathTimer(int time)
    {
        yield return new WaitForSeconds(time);
        Dead();
    }

    public IEnumerator PoisonEffect(int time, int damage)
    {
        //posionEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            Debug.Log("Poison damage on " + this.gameObject.name + " for " + damage + " damage");
            health -= damage;  //Maybe play a sound effect
            yield return new WaitForSeconds(1f); 
        }
    }

    public IEnumerator BurningEffect(int time, int damage)
    {
        //burningEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            Debug.Log("Fire damage on " + this.gameObject.name + " for " + damage + " damage");
            health -= damage;  //Maybe play a sound effect
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator SharkToothEffect(int time, int damage)
    {
        //burningEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            Debug.Log("Shark tooth damage on " + this.gameObject.name + " for " + damage + " damage");
            health -= damage;  //Maybe play a sound effect
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator VolcanicShieldEffect(int time, int damage)
    {
        //burningEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            Debug.Log("Volcanic Shield damage on " + this.gameObject.name + " for " + damage + " damage");
            health -= damage;  //Maybe play a sound effect
            yield return new WaitForSeconds(1f);
        }
        volcanicShieldEffect = false;
    }

    public IEnumerator CursedBladeEffect(int time, int damage)
    {
        Debug.Log("Cursed Blade Triggered.");
        //burningEffect.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            Debug.Log("Volcanic Shield damage on " + this.gameObject.name + " for " + damage + " damage");
            health -= damage;  //Maybe play a sound effect
            yield return new WaitForSeconds(1f);
        }
        volcanicShieldEffect = false;
    }

    IEnumerator InitCoroutine(){
        yield return new WaitForEndOfFrame();
        //Code here after init
        if(_isBoss){
            health = GetHealth(15);
        }
        else{
            health = GetHealth(5);
        }
        originalHealth = health;
    }

    public int GetHealth(int b){
        int m = 3;
        if(_isBoss == true){
            m = 5;
        }
        int x = GameObject.Find("_LevelManager(Clone)").GetComponent<LevelManager>().levelCounter;

        return (m*x) + b; 
    }

    void Loot(int id)
    {
        GameObject rewardObject = rewardPool.GetPooledObject(id);
        if(id == 0){
            rewardObject.transform.position = new Vector3(this.gameObject.transform.position.x + Random.Range(-1,2), 1f, this.gameObject.transform.position.z + Random.Range(-1,2));
        }else{
            rewardObject.transform.position = this.gameObject.transform.position;
        }
        rewardObject.SetActive(true);
    }

    //A function that will attack the player when he challenges it
    //If the player wins it will activate a trigger box that will collect enemies and attack the
    private bool sharkToothEffect;

    //Volcanic Shield
    private bool volcanicShieldEffect;

    //Cursed Blade
    private bool cursedBladeEffect;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "HitBox")
        {
            DoDamage();
        }

        if (col.gameObject.name == "SharkTooth" || col.gameObject.name == "SharkTooth(Clone)")
        {
            if(sharkToothEffect == false){
                StartCoroutine(SharkToothEffect(3,1));
                sharkToothEffect = true;
            }
        }

        if(col.gameObject.name == "VolcanicShield"){
            if(volcanicShieldEffect == false){
                StartCoroutine(VolcanicShieldEffect(2,1));
                volcanicShieldEffect = true;
            }
        }

        if (col.gameObject.name == "CursedBlade" || col.gameObject.name == "CursedBlade(Clone)")
        {
            if(cursedBladeEffect == false){
                StartCoroutine(CursedBladeEffect(3,2));
                cursedBladeEffect = true;
            }
        }

        if (col.gameObject.name == "ToilAndTrouble" || col.gameObject.name == "ToilAndTrouble(Clone)")
        {
            health = 0;
        }

        if (col.gameObject.tag == "Bomb"){
            health -= 10;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.name == "SharkTooth" || col.gameObject.name == "SharkTooth(Clone)")
        {
            sharkToothEffect = false;
        }
    }
}