using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageOnSpawn : MonoBehaviour {

	public float ShakeIntensity=0f;
	
	public MeleeAttributes meleeSettings;
	
	public KnockbackAttributes knockbackSettings;

	public LayerMask affectedActorMask;
	
	private List<Transform> AlreadyAttacked = new List<Transform>();
	
	public bool DestroyAfterwards=true;
	public bool Repeating=true;
	public float AttackCooldown=0.5f;
	public float AttackDuration=0.5f;
	
	public virtual void Start () {
		if(meleeSettings.hitEffect)
			meleeSettings.hitEffect.CreatePool();
		if(meleeSettings.attackEffect)
			meleeSettings.attackEffect.CreatePool();
		
		StartCoroutine(SpawnAttack(AttackDuration,transform.forward));
	}
	
	public virtual IEnumerator SpawnAttack(float duration,Vector3 attackDirection) {
		if(meleeSettings.attackEffect) {
			Transform attackfx = meleeSettings.attackEffect.Spawn(transform.position);
			attackfx.parent = transform;
			attackfx.rotation=Quaternion.LookRotation(attackDirection);
		}
		while (duration > 0) {
			var hits = Physics.OverlapSphere(transform.position, meleeSettings.AttackRadius, affectedActorMask);
			foreach (var hit in hits)
			{
				if(transform.isInFront(hit.transform,meleeSettings.AttackAngle)) {
					if(!AlreadyAttacked.Contains(hit.transform)) {						
						if(transform.DealDamage(hit.transform,meleeSettings.AttackDamage,knockbackSettings.knockbackForce,knockbackSettings.knockbackDuration,transform)) {
							
							if(ShakeIntensity>0&&CamManager.instance.shaker)
								CamManager.instance.shaker.shake(ShakeIntensity);
							
							if(meleeSettings.hitEffect) {
								Transform attackfx = meleeSettings.hitEffect.Spawn(hit.transform.position);
								attackfx.parent = transform;
								attackfx.rotation=Quaternion.LookRotation(transform.position);
							}
							
							AlreadyAttacked.Add(hit.transform);
						}
					}
				}
			}
			
			duration -= Time.deltaTime;
			
			if (duration <= 0) {
				if(DestroyAfterwards)
					doRecycle();
				AlreadyAttacked.Clear();
				if(Repeating)
					StartCoroutine(WaitAndAttackAgain());
			}
			yield return 0;
		}
	}
	
	IEnumerator WaitAndAttackAgain() {
		yield return new WaitForSeconds(AttackCooldown);
		StartCoroutine(SpawnAttack(AttackDuration,transform.forward));
	}
	
	void doRecycle() {
		this.transform.Recycle();
	}
}
