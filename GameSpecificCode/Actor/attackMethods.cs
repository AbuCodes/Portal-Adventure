using UnityEngine;
using System.Collections;

public static class attackMethods {

	public static void spawnProjectile(projectile proj, Vector3 pos,Vector3 velocity,float shootRange=20,Transform owner=null) {
		projectile thisProjectile = proj.Spawn(pos);
		
		thisProjectile.Owner=owner;
		
		thisProjectile.startShoot(velocity,shootRange);
	}
	
	public static  bool DealDamage(this Transform transform,Transform hit,int damage,float knockbackForce,float knockbackDuration, Transform attacker) {
		var cH = hit.GetComponent<characterHealth>();
		if (cH != null)
		{
			Vector3 forceDirection = transform.DirectionTo(hit);
						
			if(cH.TakeDamage(damage,attacker)) {
				if(cH.cM)
					cH.cM.TakeKnockback(forceDirection.normalized*knockbackForce,knockbackDuration);
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	
	public static  bool DealDamageKnockback(this Transform transform,Transform hit,int damage,Vector3 knockbackForce,float knockbackDuration, Transform attacker) {
		var cH = hit.GetComponent<characterHealth>();
		if (cH != null)
		{			
			if(cH.TakeDamage(damage,attacker)) {
				if(cH.cM)
					cH.cM.TakeKnockback(knockbackForce,knockbackDuration);
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	
	public static  bool DealDamage(this Transform transform,Transform hit,int damage) {
		var cH = hit.GetComponent<characterHealth>();
		if (cH != null)
		{
			if(cH.TakeDamage(damage))
				return true;
			else
				return false;
		} else {
			return false;
		}
	}
}
