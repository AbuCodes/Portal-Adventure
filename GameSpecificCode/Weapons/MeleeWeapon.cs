using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeWeapon : Weapon {

	public MeleeAttributes meleeSettings;
	
	public KnockbackAttributes knockbackSettings;

	public LayerMask affectedActorMask;
	
	private List<Transform> AlreadyAttacked = new List<Transform>();
	
    public override void Start () {
		if(meleeSettings.hitEffect)
			meleeSettings.hitEffect.CreatePool();
		if(meleeSettings.attackEffect)
			meleeSettings.attackEffect.CreatePool();
		timeOfAttack = Time.time;
	}
	
	public override void ExecuteAttack(Vector3 attackDirection) {
		StartCoroutine(MeleeAttack(meleeSettings.AttackDuration,attackDirection));
	}
	
	public virtual IEnumerator MeleeAttack(float duration,Vector3 attackDirection) {		
		Owner.cM.isAttacking(true);
		
		Owner.cM.anim.Play(meleeSettings.attackAnimationName,meleeSettings.attackAnimationLayer);
		
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
				if(Owner.cM.body.transform.isInFront(hit.transform,meleeSettings.AttackAngle)) {
					if(!AlreadyAttacked.Contains(hit.transform)) {						
						if(transform.DealDamage(hit.transform,meleeSettings.AttackDamage,knockbackSettings.knockbackForce,knockbackSettings.knockbackDuration,Owner.transform)) {
							
							if(ShakeIntensity>0&&CamManager.instance.shaker)
								CamManager.instance.shaker.shake(ShakeIntensity);
							
							if(meleeSettings.hitEffect) {
								Transform attackfx = meleeSettings.hitEffect.Spawn(hit.transform.position);
								attackfx.rotation=Quaternion.LookRotation(Owner.transform.position);
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

		Owner.cM.isAttacking(false);
		
		AlreadyAttacked.Clear();
	}

}
