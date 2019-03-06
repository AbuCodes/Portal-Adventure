using UnityEngine;
using System.Collections;

using System.Collections.Generic;

	[System.Serializable]
	public class MeleeAttributes {
		public float AttackRadius=4f;
		public int AttackDamage=1;
		public string AttackSound="";
		public float AttackDuration=0.3f;
		public float AttackAngle=60f;
		public Transform attackEffect;
		public Transform hitEffect;
		public string attackAnimationName="Attack";
		public int attackAnimationLayer=0;
	}
	
	[System.Serializable]
	public class KnockbackAttributes {
		public float knockbackForce=0.1f;
		public float knockbackDuration=0.4f;
	}


public class EnemyAtkMelee : EnemyAttack {

	public MeleeAttributes meleeSettings;
	
	public KnockbackAttributes knockbackSettings;

	public LayerMask affectedActorMask;
	
	private List<Transform> AlreadyAttacked = new List<Transform>();
	
    public override void Start () {
    	base.Start();
		if(meleeSettings.hitEffect)
			meleeSettings.hitEffect.CreatePool();
		if(meleeSettings.attackEffect)
			meleeSettings.attackEffect.CreatePool();
	}
	
	void OnGUI () {
		if(!eAI.Log)
			return;
        GUI.Label (new Rect (50,50,400,50), "Attacking "+eAI.attacking);
    }
    
	public override void Attack(Vector3 attackDirection) {
		if(!eAI)
			return;
		StartCoroutine(MeleeAttack(meleeSettings.AttackDuration,attackDirection));
	}
	
	public virtual IEnumerator MeleeAttack(float duration,Vector3 attackDirection) {
		eAI.attacking=true;
		
		eAI.cM.isAttacking(true);
		
		eAI.cM.anim.Play(meleeSettings.attackAnimationName,meleeSettings.attackAnimationLayer);
		
		yield return new WaitForSeconds(WaitBeforeAttack);
		
		SoundManager.instance.Play(meleeSettings.AttackSound);
		
		if(meleeSettings.attackEffect) {
			Transform attackfx = meleeSettings.attackEffect.Spawn(transform.position);
			attackfx.parent = transform;
			attackfx.rotation=Quaternion.LookRotation(attackDirection);
		}
		while (duration > 0) {
			var hits = Physics.OverlapSphere(transform.position, meleeSettings.AttackRadius, affectedActorMask);
			foreach (var hit in hits)
			{
				if(eAI.cM.body.transform.isInFront(hit.transform,meleeSettings.AttackAngle)) {
					if(!AlreadyAttacked.Contains(hit.transform)) {						
						if(transform.DealDamage(hit.transform,meleeSettings.AttackDamage,knockbackSettings.knockbackForce,knockbackSettings.knockbackDuration,eAI.transform)) {
							
							if(ShakeIntensity>0&&CamManager.instance.shaker)
								CamManager.instance.shaker.shake(ShakeIntensity);
							
							if(meleeSettings.hitEffect) {
								Transform attackfx = meleeSettings.hitEffect.Spawn(hit.transform.position);
								attackfx.rotation=Quaternion.LookRotation(transform.position);
							}
							
							AlreadyAttacked.Add(hit.transform);
						}
					}
				}
			}
			
			duration -= Time.deltaTime;
			
			if (duration <= 0) {
				//FINISH
			}
			yield return 0;
		}
		
		yield return new WaitForSeconds(WaitAfterAttack);
		
		eAI.attacking=false;
		
		eAI.cM.isAttacking(false);
		
		AlreadyAttacked.Clear();
	}
}
