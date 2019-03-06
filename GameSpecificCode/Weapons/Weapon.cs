using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float AttackCooldown=0.15f;

	public float ShakeIntensity=0f;

	[HideInInspector]
	public PlayerController Owner;

	[HideInInspector]
	public float timeOfAttack;
	
	public float WaitBeforeAttack=0.0f;
	public float WaitAfterAttack=0.0f;
	
	public void Awake() {
		StartCoroutine(AddPickUpComponent());
	}
	
	public virtual void Start () {
		timeOfAttack = Time.time;
	}
	
	public virtual void Attack(Vector3 attackDirection) {
		if(timeOfAttack + AttackCooldown > Time.time)
			return;
		
		timeOfAttack = Time.time;
		
		ExecuteAttack(attackDirection);
	}
	
	public virtual void ExecuteAttack(Vector3 attackDirection) {
		//Override this with the attack
	}
	
	public virtual void InterruptCoroutines() {
        StopAllCoroutines();
	}
	
	public virtual void OnEquip() {
		
	}
	
	public virtual void OnUnequip() {
		
	}
	
	IEnumerator AddPickUpComponent() {
		 yield return 2;
		 if(!Owner) {
			var pW = gameObject.AddComponent<PickupWeapon>();
			pW.CantPickUpOnSpawn=1.5f;
		 }
	}
}
