using UnityEngine;
using System.Collections;

	[System.Serializable]
	public class Effects {
		public Transform DeathEffect;
		public string DeathSoundEffect;
		public Transform HitEffect;
		public string HitSoundEffect;
		public Transform HealEffect;
	}


public class characterHealth : MonoBehaviour {

	public int MaxHealth=5;
	public int CurrentHealth;
	
	private Flasher flasher;
	private Shake shaker;
	[HideInInspector]
	public characterMove cM;
	
	public Effects effects;
	
	public int FlashAmount=5;
	public float ShakeAmount=0.6f;
	
	float timeOfLastHurt;

	//[HideInInspector]
	//public WaveSpawner SpawnedBy;

	public bool ShouldRespawn;
	
	[HideInInspector]
	public bool Dead;
	
	public bool RecycleOnDeath=false;

	[HideInInspector]
	public Vector3 RespawnPosition;
	
	public float HurtInterval=0.2f;
	
	int OriginalLayer;
	
	Transform LastAttacker;
	
	void Start () {
		flasher=GetComponent<Flasher>();
		shaker=GetComponent<Shake>();
		cM=GetComponent<characterMove>();
		
		if(effects.DeathEffect)
			effects.DeathEffect.CreatePool();
		if(effects.HitEffect)
			effects.HitEffect.CreatePool();
		if(effects.HealEffect)
			effects.HealEffect.CreatePool();
			
		RespawnPosition=transform.position;
		
		OriginalLayer=gameObject.layer;
		CurrentHealth=MaxHealth;
		timeOfLastHurt = Time.time;
	}
	
	public void OnDeath() {
		if(ShouldRespawn)
			transform.position=RespawnPosition;
	}
	
	public void GetInvulnerable(float duration) {
		StartCoroutine(Invulnerable(duration));
	}
	
	IEnumerator Invulnerable(float duration) {
		int FlashAmount = (int) duration*10;

		if(flasher)
			flasher.Flash(0,FlashAmount);

		while (duration > 0) {
			timeOfLastHurt = Time.time;

			duration -= Time.deltaTime;
			
			yield return 0;
		}
	}
	
	public void HealDamage(int damage) {
		if(CurrentHealth==MaxHealth)
			return;

		CurrentHealth = Mathf.Min(CurrentHealth+damage, MaxHealth);

		if(effects.HealEffect)
			effects.HealEffect.Spawn(transform.position);
			
		SendMessage("OnHeal", null, SendMessageOptions.DontRequireReceiver);
	}
	
	public void HealDamagePct(float damage) {			
		damage = MaxHealth*damage;
		HealDamage((int)damage);
	}
	
	public bool TakeDamage(int damage, int flashAm ,float shakeAm, Transform Attacker) {
		if(timeOfLastHurt + HurtInterval > Time.time)
			return false;
			
		timeOfLastHurt = Time.time;
		
		CurrentHealth-=damage;
		
		if(CurrentHealth<0) {
			Die();
			return true;
		}
		
		if(effects.HitSoundEffect!="")
			SoundManager.instance.Play(effects.HitSoundEffect);
		
		if(effects.HitEffect)
			effects.HitEffect.Spawn(transform.position);
		
		if(flasher)
			flasher.Flash(0,flashAm);
		
		if(shaker)
			shaker.shake(shakeAm);
		
		LastAttacker = Attacker;
		
		SendMessage("OnDamage", Attacker, SendMessageOptions.DontRequireReceiver);
		
		return true;
	}
	
	public bool TakeDamage(int damage, Transform Attacker) {
		if(TakeDamage(damage,FlashAmount,ShakeAmount,Attacker))
			return true;
		else
			return false;
	}
	
	public bool TakeDamage(int damage) {
		if(TakeDamage(damage,FlashAmount,ShakeAmount,null))
			return true;
		else
			return false;
	}
	
	public void Die() {
		if(cM.anim)
			cM.anim.SetBool("Dead",true);
		Dead=true;
		gameObject.layer=12;
		if(effects.DeathSoundEffect!="")
			SoundManager.instance.Play(effects.DeathSoundEffect);
			
		StartCoroutine(Death());
	}
	
	IEnumerator Death()
    {		
        if(effects.DeathEffect)
			effects.DeathEffect.Spawn(transform.position);
        
        SendMessage("OnDeath", LastAttacker, SendMessageOptions.DontRequireReceiver);
        
        if(flasher)
			flasher.Flash(-1,6);
			
        yield return new WaitForSeconds(0.3f);
        
        if(ShouldRespawn) {
        	transform.position = RespawnPosition;
        	Dead=false;
        	SendMessage("OnEnabled", null, SendMessageOptions.DontRequireReceiver);
        } else if(RecycleOnDeath) {
        	doRecycle();
        }
    }
    
    void doRecycle() {
    	gameObject.layer = OriginalLayer;
    	Dead=false;
    	if(cM.anim)
			cM.anim.SetBool("Dead",false);
    	this.Recycle();
    }
 
	public float GetHealthPct() {
		return CurrentHealth*1.0f/MaxHealth*1.0f;
	}
}